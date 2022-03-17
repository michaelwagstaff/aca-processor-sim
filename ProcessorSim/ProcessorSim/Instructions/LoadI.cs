using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadI : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private int value;
    public LoadI(Register register, int value)
    {
        this.reg = register;
        this.value = value;
        this.executionType = ExecutionTypes.SimpleArithmetic;
        this.reg.available = false;
    }
    public bool execute(Resources resources)
    {
        result = value;
        targetRegister = reg;
        return true;
    }
}