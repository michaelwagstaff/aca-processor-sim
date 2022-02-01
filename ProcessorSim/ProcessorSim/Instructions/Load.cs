using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Load
{
    public bool execute(Resources resources, Register register, int immediateoperand)
    {
        register.setValue(resources.dataMemory[immediateoperand].getValue());
        return true;
    }
}