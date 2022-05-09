using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class MemoryUnit
{
    public bool memory(Resources resources, Instruction instruction)
    {
        if (instruction != null)
        {
            if (resources.verbose)
            {
                Console.WriteLine("  Instruction {0}", instruction);
                Console.WriteLine("  Actual Output Register: {0}", instruction.targetRegister);
            }

            if (instruction.executionType == ExecutionTypes.SimpleArithmetic || instruction.executionType == ExecutionTypes.ComplexArithmetic)
            {
                if (instruction.targetRegister != null)
                {
                    resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
                }
            }
            else if (instruction.executionType == ExecutionTypes.LoadStore)
            {
                if (resources.verbose)
                {
                    /*
                    Console.WriteLine("Memory stage for load/store instruction {0}",
                        instruction);
                        */
                }

                if (instruction.targetRegister != null)
                {
                    // i.e. we're doing a load
                    // Do loads get forwarded here or do we have to wait until writeback??
                    resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
                }
                else
                {
                    StoreInstruction tempInstruction = (StoreInstruction) instruction;
                    // Need to do this to be able to access memory index
                    // resources.dataMemory[tempInstruction.memoryIndex].setValue(instruction.result);
                    resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
                    resources.CDBBroadcastMemoryAddress(instruction.reorderBuffer, ((StoreInstruction)instruction).memoryIndex);
                }
            }
            resources.instructionsWaitingMemory.Remove(instruction);
        }

        return true;
    }
}