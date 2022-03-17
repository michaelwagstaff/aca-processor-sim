using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadR : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private Register memoryIndex;
    public LoadR(Register register, Register memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
        this.reg.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        result = instruction.getVal(resources, memoryIndex);
        targetRegister = reg;
        return true;
    }
}