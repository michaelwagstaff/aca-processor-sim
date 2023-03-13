using ProcessorSim;
using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
using ProcessorSim.Instructions;

public class VLoadI : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int[] vectorResult { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;
    private int[] value;
    public VLoadI(Register register, int[] value)
    {
        inputRegisters = new List<Register>();
        targetRegister = register;
        this.value = new int[4];
        this.executionType = ExecutionTypes.Vector;
    }
    public bool execute(Resources resources, List<int> args)
    {
        vectorResult = value;
        return true;
    }
}