using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadI : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    private Register reg;
    private int value;
    public LoadI(Register register, int value)
    {
        this.reg = register;
        targetRegister = reg;
        this.value = value;
        this.executionType = ExecutionTypes.SimpleArithmetic;
        this.reg.available = false;
    }
    public bool execute(Resources resources)
    {
        result = value;
        return true;
    }
}