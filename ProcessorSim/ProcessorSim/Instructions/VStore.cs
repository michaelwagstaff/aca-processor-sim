using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class VStore : VInstruction, StoreInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int[] vectorResult { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;
    public int memoryIndex { get; set; }
    public VStore(Register register, int memoryIndex)
    {
        inputRegisters = new List<Register>();
        this.inputRegisters.Add(register);
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.Vector;
    }
    public bool execute(Resources resources, List<int> args)
    {
        vectorResult = args.ToArray();
        return true;
    }
}