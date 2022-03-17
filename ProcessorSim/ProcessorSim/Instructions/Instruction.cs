using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface Instruction
{
    ExecutionTypes executionType { get; set; }
    Register targetRegister { get; set; }
    int result { get; set; }
    public bool execute(Resources resources)
    {
        return true;
    }
    public bool memory(Resources resources)
    {
        if (this.executionType == ExecutionTypes.SimpleArithmetic)
        {
            resources.forwardedResults[targetRegister] = result;
        }
        return true;
    }
    public bool writeback(Resources resources)
    {
        if (this.executionType == ExecutionTypes.SimpleArithmetic)
        {
            targetRegister.setValue(result);
        }
        return true;
    }

    public int getVal(Resources resources, Register register)
    {
        int val = register.getValue();
        if (resources.forwardedResults[register] != null)
        {
            val = (int) resources.forwardedResults[register];
            resources.forwardedResults[register] = null;
        }

        return val;
    }
}