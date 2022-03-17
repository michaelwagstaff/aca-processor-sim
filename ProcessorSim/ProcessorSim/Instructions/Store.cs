using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Store : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private int memoryIndex;
    public Store(Register register, int memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int val = instruction.getVal(resources, reg);
        resources.dataMemory[this.memoryIndex].setValue(val);
        return true;
    }
}