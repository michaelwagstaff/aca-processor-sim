using ProcessorSim;
using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class VLoadR : VInstruction, RegisterLoadStore
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int[] vectorResult { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;
    public Register memoryIndexRegister { get; set; }
    public VLoadR(Register register, Register memoryIndexRegister)
    {
        inputRegisters = new List<Register>();
        targetRegister = register;
        this.memoryIndexRegister = memoryIndexRegister;
        this.executionType = ExecutionTypes.Vector;
    }
    public bool execute(Resources resources, List<int> args)
    {
        int memoryIndex = args[0];
        vectorResult = new int[4];
        for (int i = 0; i < 4; i++)
        {
            vectorResult[i] = resources.dataMemory[args[0]+i].getValue();
        }
        return true;
    }
}