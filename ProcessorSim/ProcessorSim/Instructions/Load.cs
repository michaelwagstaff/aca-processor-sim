using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Load
{
    private Register reg;
    private int memoryIndex;
    public Load(Register register, int memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
    }
    public bool execute(Resources resources)
    {
        this.reg.setValue(resources.dataMemory[this.memoryIndex].getValue());
        return true;
    }
}