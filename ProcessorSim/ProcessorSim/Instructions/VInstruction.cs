using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface VInstruction : Instruction
{
    ExecutionTypes executionType { get; set; }
    Register targetRegister { get; set; }
    int[] vectorResult { get; set; }
    int registerFile { get; set; }
    List<Register> inputRegisters { get; set; }
    int reorderBuffer { get; set; }
    public bool execute(Resources resources, List<int> vals)
    {
        return true;
    }
}