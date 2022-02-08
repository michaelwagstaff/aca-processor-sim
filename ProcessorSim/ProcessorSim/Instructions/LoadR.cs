using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadR : Instruction
{
    private Register reg;
    private Register memoryIndex;
    public LoadR(Register register, Register memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;

        this.reg.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources)
    {
        this.reg.setValue(resources.dataMemory[this.memoryIndex.getValue()].getValue());
        return true;
    }
}