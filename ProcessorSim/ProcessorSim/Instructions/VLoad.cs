using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class VLoad : VInstruction, ImmediateMemoryLoadStore
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
    public VLoad(Register register, int memoryIndex)
    {
        inputRegisters = new List<Register>();
        targetRegister = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int> args)
    {
        for (int i = 0; i < 4; i++)
        {
            vectorResult[i] = resources.dataMemory[memoryIndex+i].getValue();
        }

        return true;
    }
}