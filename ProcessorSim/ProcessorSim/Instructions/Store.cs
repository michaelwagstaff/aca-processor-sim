using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Store : StoreInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    private Register reg;
    public int memoryIndex { get; set; }
    public Store(Register register, int memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        result = instruction.getVal(resources, reg);
        return true;
    }
}