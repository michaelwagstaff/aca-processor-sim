using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Store : Instruction
{
    private Register reg;
    private int memoryIndex;
    public Store(Register register, int memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
    }
    public bool execute(Resources resources)
    {
        resources.dataMemory[this.memoryIndex].setValue(this.reg.getValue());
        return true;
    }
}