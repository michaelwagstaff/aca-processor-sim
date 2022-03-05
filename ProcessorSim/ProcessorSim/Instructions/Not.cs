using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Not : Instruction
{
    public ExecutionTypes executionType { get; set; }
    private Register reg;
    public Not(Register reg)
    {
        this.reg = reg;
        this.executionType = ExecutionTypes.Arithmetic;
    }

    public bool execute(Resources resources)
    {
        if (this.reg.getValue() == 1)
        {
            this.reg.setValue(0);
        }
        else
        {
            this.reg.setValue(1);
        }
        return true;
    }
}