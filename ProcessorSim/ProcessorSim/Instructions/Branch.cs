using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Branch
{
    private Resources resources;
    private Register newAddress;

    public Branch(Resources resources, Register newAddress)
    {
        this.resources = resources;
        this.newAddress = newAddress;
    }

    public bool execute()
    {
        this.resources.pc.setValue(newAddress.getValue());
        return true;
    }
}