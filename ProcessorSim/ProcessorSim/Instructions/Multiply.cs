using System.Runtime.CompilerServices;
using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Multiply : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg1, reg2;
    public Multiply(Register register1, Register register2)
    {
        reg1 = register1;
        reg2 = register2;
        this.executionType = ExecutionTypes.ComplexArithmetic;
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int val1 = instruction.getVal(resources, reg1);
        int val2 = instruction.getVal(resources, reg2);
        reg1.setValue(val1 * val2);
        return true;
    }
}