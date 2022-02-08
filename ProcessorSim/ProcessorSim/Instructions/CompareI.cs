using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class CompareI : Instruction
{
    private Register flag, reg1;
    private int value;
    public CompareI(Register flag, Register register1, int value)
    {
        this.flag = flag;
        this.reg1 = register1;
        this.value = value;
    }

    public bool execute(Resources resources)
    {
        this.flag.setValue((this.reg1.getValue() == value) ? 1 : 0);
        return true;
    }
}