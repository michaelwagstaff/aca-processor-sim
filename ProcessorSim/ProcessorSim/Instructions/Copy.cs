using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Copy : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private Register dest;
    public Copy(Register register, Register destination)
    {
        this.reg = register;
        this.dest = destination;
        this.executionType = ExecutionTypes.LoadStore;
        this.dest.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int val = instruction.getVal(resources, reg);
        this.dest.setValue(val);
        return true;
    }
}