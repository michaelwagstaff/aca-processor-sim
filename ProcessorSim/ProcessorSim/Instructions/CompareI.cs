using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class CompareI : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public (ExecutionTypes, int) reservationStation { get; set; }
    private Register reg1;
    private int value;
    public CompareI(Register flag, Register register1, int value)
    {
        this.targetRegister = flag;
        this.reg1 = register1;
        this.value = value;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int val1 = instruction.getVal(resources, reg1);
        this.result = (val1 == value) ? 1 : 0;
        return true;
    }
}