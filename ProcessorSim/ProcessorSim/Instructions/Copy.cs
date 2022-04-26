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
    public (ExecutionTypes, int) reservationStation { get; set; }
    private Register reg;
    private Register dest;
    public Copy(Register destination, Register register)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register);
        this.reg = register;
        this.targetRegister = destination;
        this.executionType = ExecutionTypes.SimpleArithmetic;
        this.targetRegister.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        result = args[0];
        return true;
    }
}