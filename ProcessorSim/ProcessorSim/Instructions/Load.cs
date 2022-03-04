using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Load : Instruction
{
    public ExecutionTypes executionType { get; set; }
    private Register reg;
    private int memoryIndex;
    public Load(Register register, int memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
        this.reg.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources)
    {
        this.reg.setValue(resources.dataMemory[this.memoryIndex].getValue());
        return true;
    }
}