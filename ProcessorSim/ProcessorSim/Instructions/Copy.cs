using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Copy : Instruction
{
    private Register reg;
    private Register dest;
    public Copy(Register register, Register destination)
    {
        this.reg = register;
        this.dest = destination;

        this.dest.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources)
    {
        this.dest.setValue(this.reg.getValue());
        return true;
    }
}