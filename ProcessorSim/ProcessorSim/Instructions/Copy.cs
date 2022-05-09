using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Copy : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register dest;
    public Copy(Register destination, Register register)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register);
        this.targetRegister = destination;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = args[0];
        return true;
    }
}