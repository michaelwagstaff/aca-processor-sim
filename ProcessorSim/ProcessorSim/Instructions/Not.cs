using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Not : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    public Not(Register target, Register reg)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(reg);
        this.targetRegister = target;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }

    public bool execute(Resources resources, List<int> args)
    {
        int val = args[0];

        this.result = val == 1 ? 0 : 1;
        return true;
    }
}