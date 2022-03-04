using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class StoreR : Instruction
{
    public ExecutionTypes executionType { get; set; }
    private Register reg;
    private Register memoryIndex;
    public StoreR(Register register, Register memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources)
    {
        resources.dataMemory[this.memoryIndex.getValue()].setValue(this.reg.getValue());
        return true;
    }
}