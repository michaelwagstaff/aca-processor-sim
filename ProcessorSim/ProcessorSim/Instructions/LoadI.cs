using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadI
{
    private Register reg;
    private int value;
    public LoadI(Register register, int value)
    {
        this.reg = register;
        this.value = value;

        this.reg.available = false;
    }
    public bool execute(Resources resources)
    {
        this.reg.setValue(this.value);
        return true;
    }
}