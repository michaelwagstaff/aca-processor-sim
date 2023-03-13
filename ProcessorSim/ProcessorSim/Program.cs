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
    static bool specExEnabled;
    static int superscalarCount;
    public static void Main(string[] args)
    {
        verbose = false;
        specExEnabled = true;
        nextInstructionNeedsNewRegister = false;
        superscalarCount = 4;
        Resources resources = new Resources(32, 512, 1024, verbose, superscalarCount, specExEnabled);
        resources.setExecutionUnits(1,(int)Math.Floor(superscalarCount/(double)2) + 1,(int)Math.Floor(superscalarCount/(double)2) + 1,(int)Math.Floor(superscalarCount/(double)2) + 1, Math.Min(2, superscalarCount));
        loadProgram(resources);
        instructionRegister = null;
        bool fetchSuccessful = true;
        int fetchFailedCount = 0;
        while (fetchFailedCount < 1)
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
        // StreamReader reader = new StreamReader(@"Programs/bubblesort-perf.mpl");
        // StreamReader reader = new StreamReader(@"Programs/bubblesort-unrolled-perf.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact-safe.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact-limited.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact-unrolled-limited.mpl");
        // StreamReader reader = new StreamReader(@"Programs/gcd-original.mpl");
        // StreamReader reader = new StreamReader(@"Programs/gcd-original-perf.mpl");
        // StreamReader reader = new StreamReader(@"Programs/hamming.mpl");
        // StreamReader reader = new StreamReader(@"Programs/add.mpl");
        // StreamReader reader = new StreamReader(@"Programs/add-perf.mpl");
        StreamReader reader = new StreamReader(@"Programs/vectoradd.mpl");
        // StreamReader reader = new StreamReader(@"Programs/vectoradd-perf.mpl");
        // StreamReader reader = new StreamReader(@"Programs/vectoradd-unrolled-perf.mpl");
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
        if (specExEnabled)
        {
            execute(resources);
            bool haltPipeline =
                decode(resources); // This pipeline only stops in an emergency. A potential branch is not an emergency
            if (!haltPipeline)
            {
                fetch(resources);
                if (instructionRegister == -1)
                {
                    return false;
                }
            }
        }
        else
        {
            if (execute(resources) != 1 && resources.reservationStations[ExecutionTypes.Branch].hasSpace()) // If pipeline flush isn't occuring
            {
                if (!resources.reorderBuffer.containsBranch())
                {
                    bool haltPipeline = decode(resources); // TODO: Improve this
                    foreach ((int, (bool, (bool, int, int))) instruction in resources.instructionsWaitingDecode)
                    {
                        if (instruction.Item2.Item2.Item1)
                            haltPipeline = true;
                    }

                    if (!haltPipeline)
                        fetch(resources);
                    if (instructionRegister == -1)
                    {
                        return false;
                    }
                }
            }
        }
        if(verbose)
        {
            Console.WriteLine("Current Register Mapping:");
            //resources.registerFile.printMapping();
        }
        
        resources.monitor.incrementCyclesTaken();
        if (verbose)
        {
            Console.WriteLine("Tick Ended: Press enter to continue...");
            Console.ReadLine();
        }
        //Console.WriteLine("TICK!");

        return true;
    }

    public static void fetch(Resources resources)
    {
        while (resources.instructionsWaitingDecode.Count < superscalarCount)
        {
            resources.instructionsWaitingDecode.Add(resources.fetchUnits[0].fetch(resources, superscalarCount));
        }
    }

    public static bool decode(Resources resources)
    {
        // Returns true if we need to halt.
        if(verbose)
            Console.WriteLine("Decode Debug:");
        (int, (bool, (bool, int, int)))[] instructionArray = resources.instructionsWaitingDecode.ToArray();
        foreach ((int, (bool, (bool, int, int))) instruction in instructionArray)
        {
            int? instructionRegister = instruction.Item1;
            bool newRegisterNeeded = instruction.Item2.Item1;
            bool branch = instruction.Item2.Item2.Item1;
            Instruction instructionObject =
                resources.decodeUnits[0].decode(resources, instructionRegister, instruction.Item2.Item2);
            bool result = resources.reservationStations[instructionObject.executionType].addItem(instructionObject);
            resources.instructionsWaitingDecode.Remove(instruction);
            if (!result)
                return true;
            if (!specExEnabled && branch)
                return true;
        }

        try
        {
            //if (verbose)
                //resources.registerFile.printMapping();
        } catch {}

        return false;
    }

    public static int execute(Resources resources)
    {
        int returnVal = 0;
        if (verbose)
        {
            Console.WriteLine("Execution Debug:");
            Console.WriteLine("  Initial Reservation Station State:");
            // resources.reservationStation.printContents();
        }

        //try
        //{
            foreach (ExecutionTypes executionType in Enum.GetValues(typeof(ExecutionTypes)))
            {
                if (resources.executionUnits.ContainsKey(executionType))
                {
                    for (int i = 0; i < resources.executionUnits[executionType].Count; i++)
                    {
                        if (resources.executionUnits[executionType][i].blocked)
                        {
                            Console.WriteLine(  "Unit Blocked!");
                            resources.executionUnits[executionType][i].execute(resources, (new Blank(), new List<int>()));
                        }
                        else
                        {
                            if (executionType == ExecutionTypes.Branch)
                            {
                                bool? nullablePipelineFlush = resources.executionUnits[executionType][i]
                                    .execute(resources, resources.reservationStations[ExecutionTypes.Branch].getItem());
                                bool pipelineFlush =
                                    nullablePipelineFlush == null ? false : (bool) nullablePipelineFlush;
                            }
                            else
                            {
                                
                                resources.executionUnits[executionType][i]
                                    .execute(resources, resources.reservationStations[executionType].getItem());
                            }
                        }
                    }
                }
            }
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
        resources.reorderBuffer.commit(superscalarCount);
    }
}