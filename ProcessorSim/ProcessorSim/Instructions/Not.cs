using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Not : Instruction
{
    private Register reg;
    public Not(Register reg)
    {
        this.reg = reg;
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