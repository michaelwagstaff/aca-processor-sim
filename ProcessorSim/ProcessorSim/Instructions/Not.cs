using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Not : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    private Register reg;
    public Not(Register target, Register reg)
    {
        this.reg = reg;
        this.targetRegister = target;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int val = instruction.getVal(resources, reg);

        this.result = val == 1 ? 0 : 1;
        return true;
    }
}