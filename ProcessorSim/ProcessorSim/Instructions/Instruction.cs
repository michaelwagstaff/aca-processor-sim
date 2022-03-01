using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public abstract class Instruction
{
    public ExecutionTypes executionType;
    public bool execute(Resources resources)
    {
        return true;
    }
}