using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class CommitUnit
{
    public bool commit(Resources resources, Instruction instruction)
    {
        //Register actualTargetRegister =
        //    resources.registerFile.getPhysicalRegister(instruction.registerFile, instruction.targetRegister);
        if ((instruction.executionType == ExecutionTypes.SimpleArithmetic || instruction.executionType == ExecutionTypes.ComplexArithmetic) && instruction.targetRegister != null)
        {
            if (resources.verbose)
            {
                Console.WriteLine("  Instruction {0}: saving result {1} into register {2}", instruction, instruction.result,
                    instruction.targetRegister);
            }
            instruction.targetRegister.setValue(instruction.result);
            if (instruction.GetType() == typeof(Add))
            {
                //Console.Write("");
            }
            resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
        }
        else if (instruction.executionType == ExecutionTypes.LoadStore)
        {
            if (instruction.targetRegister != null)
            {
                // Load operation
                instruction.targetRegister.setValue(instruction.result);
                resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
                // Depending on when we get result from memory unit may need to remove data hazard marker here
            }
            else
            {
                
            }
        }
        else if (instruction.executionType == ExecutionTypes.Branch)
        {
            // Add branch logic
            resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
        }
        else if(instruction.executionType == ExecutionTypes.General)
        {
            if (instruction.GetType() == typeof(End))
            {
                Console.WriteLine();
                Console.WriteLine("-- Fin --");
                Console.WriteLine();
                Console.WriteLine("Program Stats:");
                resources.monitor.printStats();
                Environment.Exit(0);
            }
            resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
        }
        else if (instruction.executionType == ExecutionTypes.Vector)
        {
            resources.CDBVectorBroadcast(instruction.reorderBuffer, ((VInstruction)instruction).vectorResult);
            resources.reorderBuffer.notifyCommitted(instruction.reorderBuffer);
            if (instruction.targetRegister != null || instruction.executionType == ExecutionTypes.SimpleArithmetic)
            {
                instruction.targetRegister.setValue(((VInstruction)instruction).vectorResult);
            }
            if (instruction.GetType() == typeof(VStore) || instruction.GetType() == typeof(VStoreR))
            {
                for (int i = 0; i < 4; i++)
                {
                    resources.dataMemory[((StoreInstruction) instruction).memoryIndex + i].setValue(
                        ((VInstruction) instruction).vectorResult[i]);
                }
            }
        }
        return true;
    }
}