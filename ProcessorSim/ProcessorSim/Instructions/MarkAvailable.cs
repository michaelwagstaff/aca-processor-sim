using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class MarkAvailable
{
    private Register reg;

    public MarkAvailable(Register register)
    {
        this.reg = register;
    }

    public bool execute()
    {
        this.reg.available = true;
        return true;
    }
}