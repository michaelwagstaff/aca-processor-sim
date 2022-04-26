using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class CompareLT : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public (ExecutionTypes, int) reservationStation { get; set; }
    private Register reg1, reg2;
    public CompareLT(Register flag, Register register1, Register register2)
    {
        this.targetRegister = flag;
        this.reg1 = register1;
        this.reg2 = register2;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int val1 = instruction.getVal(resources, reg1);
        int val2 = instruction.getVal(resources, reg2);

        this.result = val1 < val2 ? 1 : 0;
        return true;
    }
}