using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface Instruction
{
    public bool execute(Resources resources)
    {
        return true;
    }
}