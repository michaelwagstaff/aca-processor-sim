using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface Instruction
{
    ExecutionTypes executionType { get; set; }
    Register targetRegister { get; set; }
    int result { get; set; }
    int registerFile { get; set; }
    List<Register> inputRegisters { get; set; }
    int reorderBuffer { get; set; }
    public bool execute(Resources resources, List<int> vals)
    {
        return true;
    }
}