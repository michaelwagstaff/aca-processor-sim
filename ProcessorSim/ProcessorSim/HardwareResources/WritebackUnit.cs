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
        }

        resources.instructionWaitingWriteback = null;
        return true;
    }
}