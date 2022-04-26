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
    public (ExecutionTypes, int) reservationStation { get; set; }
    private Register reg;
    public int memoryIndex { get; set; }
    public Load(Register register, int memoryIndex)
    {
        this.reg = register;
        targetRegister = reg;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
        this.reg.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = resources.dataMemory[memoryIndex].getValue();
        return true;
    }
}