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
    public int reorderBuffer { get; set; }
    private Register reg1, reg2;
    public CompareLT(Register flag, Register register1, Register register2)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register1);
        inputRegisters.Add(register2);
        this.targetRegister = flag;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        int val1 = args[0];
        int val2 = args[1];

        this.result = val1 < val2 ? 1 : 0;
        return true;
    }
}