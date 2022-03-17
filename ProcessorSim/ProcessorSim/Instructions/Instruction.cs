using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface Instruction
{
    ExecutionTypes executionType { get; set; }
    public bool execute(Resources resources)
    {
        return true;
    }
    public bool memory(Resources resources)
    {
        return true;
    }
    public bool writeback(Resources resources)
    {
        return true;
    }
}