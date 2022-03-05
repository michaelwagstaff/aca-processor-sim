using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadI : Instruction
{
    public ExecutionTypes executionType { get; set; }
    private Register reg;
    private int value;
    public LoadI(Register register, int value)
    {
        this.reg = register;
        this.value = value;
        this.executionType = ExecutionTypes.LoadStore;
        this.reg.available = false;
    }
    public bool execute(Resources resources)
    {
        this.reg.setValue(this.value);
        return true;
    }
}