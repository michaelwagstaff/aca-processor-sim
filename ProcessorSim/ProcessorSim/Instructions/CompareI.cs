using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class CompareI : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg1;
    private int value;
    public CompareI(Register flag, Register register1, int value)
    {
        this.targetRegister = flag;
        this.reg1 = register1;
        this.value = value;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources)
    {
        int val1 = reg1.getValue();
        if (resources.forwardedResults[reg1] != null)
        {
            val1 = (int) resources.forwardedResults[reg1];
            resources.forwardedResults[reg1] = null;
        }
        this.result = (val1 == value) ? 1 : 0;
        return true;
    }
}