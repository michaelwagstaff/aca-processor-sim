using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Store : StoreInstruction, ImmediateMemoryLoadStore
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;
    public int memoryIndex { get; set; }
    public Store(Register register, int memoryIndex)
    {
        inputRegisters = new List<Register>();
        this.inputRegisters.Add(register);
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        result = args[1];
        return true;
    }
}