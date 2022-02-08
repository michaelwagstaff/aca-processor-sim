using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class CondBranch
{
    private Resources resources;
    private Register flag;
    private Register newAddress;

    public CondBranch(Resources resources, Register flag, Register newAddress)
    {
        this.resources = resources;
        this.flag = flag;
        this.newAddress = newAddress;
    }

    public bool execute()
    {
        if(this.flag.getValue() == 1)
            this.resources.pc.setValue(newAddress.getValue());
        return true;
    }
}