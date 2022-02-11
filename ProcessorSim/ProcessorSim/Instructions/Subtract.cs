using System.Runtime.CompilerServices;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Subtract : Instruction
{
    private Register reg1, reg2;
    public Subtract(Register register1, Register register2)
    {
        reg1 = register1;
        reg2 = register2;
    }
    public bool execute(Resources resources)
    {
        reg1.setValue(reg1.getValue() - reg2.getValue());
        return true;
    }
}