using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class WritebackUnit
{
    public bool writeback(Resources resources, Instruction instruction)
    {
        if (instruction.executionType == ExecutionTypes.SimpleArithmetic && instruction.targetRegister != null)
        {
            if (resources.verbose)
            {
                Console.WriteLine("Writeback stage for instruction {0}: saving result {1} into register {2}", instruction, instruction.result,
                    instruction.targetRegister);
            }
            instruction.targetRegister.setValue(instruction.result);
            resources.dataHazards[instruction.targetRegister] = false; // Should be redundant!
            // Would rather not take chances though
        }
        else if (instruction.executionType == ExecutionTypes.LoadStore)
        {
            if (instruction.targetRegister != null)
            {
                // Load operation
                instruction.targetRegister.setValue(instruction.result);
                resources.dataHazards[instruction.targetRegister] = false; // 
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