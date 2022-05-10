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
                    if (instruction.GetType() == typeof(Add))
                    {
                        Console.Write("");
                    }
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
                    resources.dataMemory[((StoreInstruction) instruction).memoryIndex].setValue(instruction.result);
                    resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
                }
            }
            else if (instruction.executionType == ExecutionTypes.General || instruction.executionType == ExecutionTypes.Branch)
            {
                resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
            }
            else if (instruction.executionType == ExecutionTypes.Vector)
            {
                if (instruction.GetType() == typeof(VLoad) || instruction.GetType() == typeof(VLoadI)
                    || instruction.GetType() == typeof(VAdd) || instruction.GetType() == typeof(VLoadR))
                {
                    resources.CDBVectorBroadcast(instruction.reorderBuffer, ((VInstruction)instruction).vectorResult);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        resources.dataMemory[((StoreInstruction) instruction).memoryIndex+i].setValue(
                            ((VInstruction)instruction).vectorResult[i]);
                    }
                    resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
                }
            }
            else if (instruction.executionType == ExecutionTypes.General)
            {
                resources.CDBBroadcast(instruction.reorderBuffer, -1);
            }
            resources.instructionsWaitingMemory.Remove(instruction);
        }

        return true;
    }
}