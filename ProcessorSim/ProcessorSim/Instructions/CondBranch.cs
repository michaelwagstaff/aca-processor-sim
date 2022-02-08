using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class CondBranch : Instruction
{
    private Register flag;
    private Register newAddress;

    public CondBranch(Register flag, Register newAddress)
    {
        this.flag = flag;
        this.newAddress = newAddress;
    }

    public bool execute(Resources resources)
    {
        if (this.flag.getValue() == 1)
        {
            resources.pc.setValue(newAddress.getValue());
        }
        return true;
    }
}