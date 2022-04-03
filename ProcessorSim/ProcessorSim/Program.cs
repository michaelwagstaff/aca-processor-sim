using System.Data;
using System.Runtime.CompilerServices;
using ProcessorSim.Instructions;
using ProcessorSim.HardwareResources;
using System.Threading;
using ProcessorSim.Enums;

namespace ProcessorSim;

class ProcessorSim
{
    static int? instructionRegister;
    static bool nextInstructionNeedsNewRegister;
    static bool verbose;
    static int superscalarCount;
    public static void Main(string[] args)
    {
        verbose = false;
        nextInstructionNeedsNewRegister = false;
        superscalarCount = 2;
        Resources resources = new Resources(32, 512, 1024, verbose, superscalarCount);
        resources.setExecutionUnits(1,1,1,1);
        loadProgram(resources);
        instructionRegister = null;
        bool fetchSuccessful = true;
        int fetchFailedCount = 0;
        while (fetchFailedCount < 2)
        {
            fetchSuccessful = tick(resources);
            if (!fetchSuccessful)
                fetchFailedCount++;
            Thread.Sleep(10);
        }
    }

    public static void loadProgram(Resources resources)
    {
        // StreamReader reader = new StreamReader(@"Programs/bubblesort.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact.mpl");
        StreamReader reader = new StreamReader(@"Programs/fact-safe.mpl");
        // StreamReader reader = new StreamReader(@"Programs/gcd-original.mpl");
        // StreamReader reader = new StreamReader(@"Programs/vectoradd.mpl");
        // StreamReader reader = new StreamReader(@"Programs/vectormult-safe.mpl");
        int i = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            resources.instructionMemory[i].setInstruction(line);
            i++;
        }
        resources.pc.setValue(0);
    }
    public static bool tick(Resources resources)
    {
        writeback(resources);
        memory(resources);
        if (execute(resources) != 1 && !resources.reservationStation.hasType(ExecutionTypes.Branch)) // If pipeline flush isn't occuring
        {
            bool haltPipeline = decode(resources); // TODO: Improve this
            foreach ((int, (bool, bool)) instruction in resources.instructionsWaitingDecode)
            {
                if (instruction.Item2.Item2)
                    haltPipeline = true;
            }
            if(!haltPipeline)
                fetch(resources);
            if (instructionRegister == -1)
            {
                return false;
            }
        }
        else if(verbose)
        {
            Console.WriteLine("Current Register Mapping:");
            resources.registerFile.printMapping();
        }
        resources.monitor.incrementCyclesTaken();
        if(verbose)
            Console.WriteLine("Tick");
        return true;
    }

    public static void fetch(Resources resources)
    {
        while (resources.instructionsWaitingDecode.Count < superscalarCount)
        {
            resources.instructionsWaitingDecode.Add(resources.fetchUnits[0].fetch(resources, verbose));
        }
    }

    public static bool decode(Resources resources)
    {
        // Returns true if we need to halt.
        (int, (bool, bool))[] instructionArray = resources.instructionsWaitingDecode.ToArray();
        foreach ((int, (bool, bool)) instruction in instructionArray)
        {
            int? instructionRegister = instruction.Item1;
            bool newRegisterNeeded = instruction.Item2.Item1;
            bool branch = instruction.Item2.Item2;
            bool result = resources.reservationStation.addItem(resources.decodeUnits[0].decode(resources, instructionRegister, newRegisterNeeded));
            resources.instructionsWaitingDecode.Remove(instruction);
            if (branch || !result)
                return true;
        }

        return false;
    }

    public static int execute(Resources resources)
    {
        int returnVal = 0;
        if (verbose)
        {
            Console.WriteLine("Execution Debug:");
            Console.WriteLine("  Initial Reservation Station State:");
            resources.reservationStation.printContents();
        }

        //try
        //{
            foreach (ExecutionTypes executionType in Enum.GetValues(typeof(ExecutionTypes)))
            {
                if (resources.executionUnits.ContainsKey(executionType))
                {
                    if (resources.executionUnits[executionType][0].blocked)
                    {
                        resources.executionUnits[executionType][0].execute(resources);
                    }
                    else
                    {
                        if (executionType == ExecutionTypes.Branch)
                        {
                            bool? nullablePipelineFlush = resources.executionUnits[executionType][0]
                                .execute(resources, resources.reservationStation.getItem(executionType));
                            bool pipelineFlush = nullablePipelineFlush == null ? false : (bool) nullablePipelineFlush;
                            // If null, then there is no pipeline flush
                            if (pipelineFlush)
                            {
                                instructionRegister = null;
                                if (instructionRegister != null)
                                {
                                    resources.registers[(int) instructionRegister].available = true;
                                    instructionRegister = null;
                                }

                                resources.reservationStation.flush(resources);
                                if (verbose)
                                    Console.WriteLine("Branch -- Pipeline Flush");
                                returnVal = 1;
                            }
                        }
                        else
                        {
                            resources.executionUnits[executionType][0]
                                .execute(resources, resources.reservationStation.getItem(executionType));
                        }
                    }
                }
            }
        /*}
        
        catch (NullReferenceException)
        {
            if (verbose)
                Console.WriteLine("Null Instruction in Pipeline");
            return -1;
        }*/

        return returnVal;
    }

    public static void memory(Resources resources)
    {
        if(verbose)
            Console.WriteLine("Memory Debug:");
        int count = resources.instructionsWaitingMemory.Count > superscalarCount ? superscalarCount : resources.instructionsWaitingMemory.Count;
        Instruction[] instructionsWaitingMemoryArray = resources.instructionsWaitingMemory.ToArray();
        for (int i = 0; i < count; i++)
        {
            if (instructionsWaitingMemoryArray[i] != null)
                resources.memoryUnit.memory(resources, instructionsWaitingMemoryArray[i]);
        }
    }
    public static void writeback(Resources resources)
    {
        if(verbose)
            Console.WriteLine("Writeback Debug:");
        int count = resources.instructionsWaitingWriteback.Count > superscalarCount ? superscalarCount : resources.instructionsWaitingWriteback.Count;
        Instruction[] instructionsWaitingWritebackArray = resources.instructionsWaitingWriteback.ToArray();
        for (int i = 0; i < count; i++)
        {
            if (instructionsWaitingWritebackArray[i] != null)
                resources.writebackUnit.writeback(resources, instructionsWaitingWritebackArray[i]);
        }
    }
}