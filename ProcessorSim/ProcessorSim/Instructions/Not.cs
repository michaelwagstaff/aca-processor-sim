using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Not : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    public Not(Register reg)
    {
        this.reg = reg;
        this.targetRegister = reg;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources)
    {
        int val = this.reg.getValue();
        if (resources.forwardedResults[reg] != null)
        {
            val = (int) resources.forwardedResults[reg];
        }

        this.result = val == 1 ? 0 : 1;
        return true;
    }
}