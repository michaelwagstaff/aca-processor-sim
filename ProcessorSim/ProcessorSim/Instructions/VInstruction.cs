using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface VInstruction
{
    ExecutionTypes executionType { get; set; }
    VRegister targetRegister { get; set; }
    int[] result { get; set; }
    int registerFile { get; set; }
    List<VRegister> inputRegisters { get; set; }
    int reorderBuffer { get; set; }
    public bool execute(Resources resources, List<int> vals)
    {
        return true;
    }
}