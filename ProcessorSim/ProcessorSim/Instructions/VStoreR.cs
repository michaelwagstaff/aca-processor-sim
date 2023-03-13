using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class VStoreR : VInstruction, RegisterLoadStore, StoreInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int[] vectorResult { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public Register memoryIndexRegister { get; set; }
    public int reorderBuffer { get; set; }
    public int memoryIndex { get; set; }
    public VStoreR(Register register, Register memoryIndex)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register);
        this.memoryIndexRegister = memoryIndex;
        this.executionType = ExecutionTypes.Vector;
    }
    public bool execute(Resources resources, List<int> args)
    {
        vectorResult = new int[4];
        for (int i = 0; i < 4; i++)
        {
            vectorResult[i] = args[i + 1];
        }
        memoryIndex = args[0];
        return true;
    }
}