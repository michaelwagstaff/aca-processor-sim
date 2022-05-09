using ProcessorSim;
using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
using ProcessorSim.Instructions;

public class VLoadI : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public VRegister targetRegister { get; set; }
    public int[] result { get; set; }
    public int registerFile { get; set; }
    public List<VRegister> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;
    private int[] value;
    public VLoadI(VRegister register, int[] value)
    {
        inputRegisters = new List<VRegister>();
        targetRegister = register;
        this.value = new int[4];
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = value;
        return true;
    }
}