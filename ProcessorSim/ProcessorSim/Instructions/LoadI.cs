using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadI : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private int value;
    public LoadI(Register register, int value)
    {
        inputRegisters = new List<Register>();
        targetRegister = register;
        this.value = value;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = value;
        return true;
    }
}