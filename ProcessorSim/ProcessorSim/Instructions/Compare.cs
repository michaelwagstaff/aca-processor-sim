using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Compare : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg1, reg2;
    public Compare(Register flag, Register register1, Register register2)
    {
        this.targetRegister = flag;
        this.reg1 = register1;
        this.reg2 = register2;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources)
    {
        int val1 = reg1.getValue();
        int val2 = reg2.getValue();
        if (resources.forwardedResults[reg1] != null)
        {
            val1 = (int) resources.forwardedResults[reg1];
            resources.forwardedResults[reg1] = null;
        }

        if (resources.forwardedResults[reg2] != null)
        {
            val2 = (int) resources.forwardedResults[reg2];
            resources.forwardedResults[reg2] = null;
        }

        this.result = (val1 == val2) ? 1 : 0;
        return true;
    }
}