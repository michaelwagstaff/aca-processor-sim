using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class MemoryUnit
{
    public bool memory(Resources resources)
    {
        Instruction instruction = resources.instructionWaitingMemory;
        if (instruction != null)
        {
            if (resources.verbose)
                Console.WriteLine("Memory Access for Instruction {0}", instruction);
            if (instruction.executionType == ExecutionTypes.SimpleArithmetic && instruction != null)
            {
                if (resources.verbose)
                {
                    Console.WriteLine("Memory stage for instruction {0}: saving result {1} to register file",
                        instruction, instruction.result);
                }

                resources.forwardedResults[instruction.targetRegister] = instruction.result;
            }
            resources.instructionWaitingWriteback = instruction;
            resources.instructionWaitingMemory = null;
        }

        return true;
    }
}