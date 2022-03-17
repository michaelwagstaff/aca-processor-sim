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
    static bool verbose;
    public static void Main(string[] args)
    {
        verbose = false;
        Resources resources = new Resources(32, 512, 1024);
        resources.setExecutionUnits(1,1,1,1);
        loadProgram(resources);
        instructionRegister = null;
        bool contFlag = true;
        while (contFlag)
        {
            contFlag = tick(resources);
            Thread.Sleep(10);
        }
    }

    public static void loadProgram(Resources resources)
    {
        // StreamReader reader = new StreamReader(@"Programs/bubblesort.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact.mpl");
        StreamReader reader = new StreamReader(@"Programs/gcd-original.mpl");
        // StreamReader reader = new StreamReader(@"Programs/vectoradd.mpl");
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
        if (execute(resources) != 1) // If pipeline flush isn't occuring
        {
            bool instructionDecoded = decode(resources, instructionRegister);
            instructionRegister = fetch(resources);
            if (instructionRegister == -1)
            {
                return false;
            }
        }
        resources.monitor.incrementCyclesTaken();
        if(verbose)
            Console.WriteLine("Tick");
        return true;
    }

    public static int fetch(Resources resources)
    {
        return resources.fetchUnits[0].fetch(resources, verbose);
    }

    public static bool decode(Resources resources, int? instructionRegister)
    {
        return resources.reservationStation.addItem(
            resources.decodeUnits[0].decode(resources, instructionRegister));
    }

    public static int execute(Resources resources)
    {
        int returnVal = 0;
        //try
        //{
            foreach (ExecutionTypes executionType in Enum.GetValues(typeof(ExecutionTypes)))
            {
                if (resources.executionUnits.ContainsKey(executionType))
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

                            resources.reservationStation.flush();
                            if (verbose)
                                Console.WriteLine("Branch -- Pipeline Flush");
                            returnVal = 1;
                        }
                    }

                    resources.executionUnits[executionType][0]
                        .execute(resources, resources.reservationStation.getItem(executionType));
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
        if(resources.instructionWaitingMemory != null)
            resources.instructionWaitingMemory.memory(resources);
    }
    public static void writeback(Resources resources)
    {
        if(resources.instructionWaitingWriteback != null)
            resources.instructionWaitingWriteback.writeback(resources);
    }
}