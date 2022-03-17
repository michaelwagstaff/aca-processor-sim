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
            if (instruction.executionType == ExecutionTypes.SimpleArithmetic)
            {
                if (resources.verbose)
                {
                    Console.WriteLine("Memory stage for arithmetic instruction {0}: saving result {1} to register file",
                        instruction, instruction.result);
                }

                resources.forwardedResults[instruction.targetRegister] = instruction.result;
                resources.dataHazards[instruction.targetRegister] = false;
            }
            if (instruction.executionType == ExecutionTypes.LoadStore)
            {
                if (resources.verbose)
                {
                    Console.WriteLine("Memory stage for load/store instruction {0}",
                        instruction);
                }

                if (instruction.targetRegister != null)
                {
                    // i.e. we're doing a load
                    // Do loads get forwarded here or do we have to wait until writeback??
                    resources.forwardedResults[instruction.targetRegister] = instruction.result;
                    resources.dataHazards[instruction.targetRegister] = false;
                }
                // Else do nothing until writeback?
            }
            resources.instructionWaitingWriteback = instruction;
            resources.instructionWaitingMemory = null;
        }

        return true;
    }
}