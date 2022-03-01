using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class Compare : Instruction
{
    private Register flag, reg1, reg2;
    public Compare(Register flag, Register register1, Register register2)
    {
        this.flag = flag;
        this.reg1 = register1;
        this.reg2 = register2;
        this.executionType = ExecutionTypes.Arithmetic;
    }

    public bool execute(Resources resources)
    {
        this.flag.setValue((this.reg1.getValue() == this.reg2.getValue()) ? 1 : 0);
        return true;
    }
}