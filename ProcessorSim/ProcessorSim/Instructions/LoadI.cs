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
    public (ExecutionTypes, int) reservationStation { get; set; }
    private Register reg;
    private int value;
    public LoadI(Register register, int value)
    {
        inputRegisters = new List<Register>();
        this.reg = register;
        targetRegister = reg;
        this.value = value;
        this.executionType = ExecutionTypes.SimpleArithmetic;
        this.reg.available = false;
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = value;
        return true;
    }
}