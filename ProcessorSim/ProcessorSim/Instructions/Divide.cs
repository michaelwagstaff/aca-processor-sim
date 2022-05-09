using System.Runtime.CompilerServices;
using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Divide : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    public Divide(Register target, Register register1, Register register2)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register1);
        inputRegisters.Add(register2);
        this.targetRegister = target;
        this.executionType = ExecutionTypes.ComplexArithmetic;
    }
    public bool execute(Resources resources, List<int> args)
    {
        int val1 = args[0];
        int val2 = args[1];
        targetRegister.setValue(val1 / val2);
        return true;
    }
}