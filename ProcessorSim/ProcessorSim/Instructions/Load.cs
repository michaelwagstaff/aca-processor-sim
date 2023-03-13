using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Load : ImmediateMemoryLoadStore
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;
    public int memoryIndex { get; set; }
    public Load(Register register, int memoryIndex)
    {
        inputRegisters = new List<Register>();
        targetRegister = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = resources.dataMemory[memoryIndex].getValue();
        return true;
    }
}