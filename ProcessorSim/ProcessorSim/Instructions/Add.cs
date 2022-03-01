using System.Runtime.CompilerServices;
using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Add : Instruction
{
    private Register reg1, reg2;
    public Add(Register register1, Register register2)
    {
        reg1 = register1;
        reg2 = register2;
        this.executionType = ExecutionTypes.Arithmetic;
    }
    public bool execute(Resources resources)
    {
        reg1.setValue(reg1.getValue() + reg2.getValue());
        return true;
    }
}