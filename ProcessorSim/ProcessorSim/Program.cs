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
    static Instruction? decodedInstruction;
    static bool verbose;
    public static void Main(string[] args)
    {
        verbose = false;
        Resources resources = new Resources(32, 512, 1024);
        resources.setExecutionUnits(1,1,1,1);
        loadProgram(resources);
        instructionRegister = null;
        decodedInstruction = null;
        while (true)
        {
            tick(resources);
            Thread.Sleep(10);
        }
    }

    public static void loadProgram(Resources resources)
    {
        StreamReader reader = new StreamReader(@"Programs/bubblesort.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact.mpl");
        // StreamReader reader = new StreamReader(@"Programs/gcd-original.mpl");
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
    public static void tick(Resources resources)
    {
        if (execute(resources, decodedInstruction) != 1) // If pipeline flush isn't occuring
        {
            decodedInstruction = decode(resources, instructionRegister);
            instructionRegister = fetch(resources);
        }
        resources.monitor.incrementCyclesTaken();
        if(verbose)
            Console.WriteLine("Tick");
    }

    public static int fetch(Resources resources)
    {
        return resources.fetchUnits[0].fetch(resources, verbose);
    }

    public static Instruction decode(Resources resources, int? instructionRegister)
    {
        return resources.decodeUnits[0].decode(resources, instructionRegister);
    }

    public static int execute(Resources resources, Instruction? instruction)
    {
        int returnVal = 0;
        try
        {
            if (instruction.executionType == ExecutionTypes.Branch)
            {
                bool pipelineFlush = (bool)resources.executionUnits[instruction.executionType][0].execute(resources, instruction);
                if (pipelineFlush)
                {
                    instructionRegister = null;
                    if (instructionRegister != null)
                    {
                        resources.registers[(int) instructionRegister].available = true;
                        instructionRegister = null;
                    }

                    decodedInstruction = null;
                    if(verbose)
                        Console.WriteLine("Branch -- Pipeline Flush");
                    returnVal = 1;
                }
            }
            else
                resources.executionUnits[instruction.executionType][0].execute(resources, instruction);
            if(verbose)
                Console.WriteLine(instruction.ToString());
        }
        catch (NullReferenceException)
        {
            if(verbose)
                Console.WriteLine("Null Instruction in Pipeline");
            return -1;
        }

        return returnVal;
    }

    public static void memory(Resources resources, Instruction instruction)
    {
        
    }
    public static void writeback(Resources resources, Instruction instruction)
    {
        
    }
}