using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class WritebackUnit
{
    public bool writeback(Resources resources, Instruction instruction)
    {
        Register actualTargetRegister =
            resources.registerFile.getPhysicalRegister(instruction.registerFile, instruction.targetRegister);
        if (instruction.executionType == ExecutionTypes.SimpleArithmetic && actualTargetRegister != null)
        {
            if (resources.verbose)
            {
                Console.WriteLine("Writeback stage for instruction {0}: saving result {1} into register {2}", instruction, instruction.result,
                    instruction.targetRegister);
            }
            actualTargetRegister.setValue(instruction.result);
        }
        else if (instruction.executionType == ExecutionTypes.LoadStore)
        {
            if (actualTargetRegister != null)
            {
                // Load operation
                actualTargetRegister.setValue(instruction.result);
                // Depending on when we get result from memory unit may need to remove data hazard marker here
            }
            else
            {
                StoreInstruction tempInstruction = (StoreInstruction) instruction;
                // Need to do this to be able to access memory index
                resources.dataMemory[tempInstruction.memoryIndex].setValue(instruction.result);
            }
        }

        resources.instructionWaitingWriteback = null;
        return true;
    }
}