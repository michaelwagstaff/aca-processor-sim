using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Branch : Instruction
{
    private Register newAddress;

    public Branch(Register newAddress)
    {
        this.newAddress = newAddress;
    }

    public bool execute(Resources resources)
    {
        resources.pc.setValue(newAddress.getValue() - 1);
        return true;
    }
}