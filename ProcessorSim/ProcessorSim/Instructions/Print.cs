using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Print : Instruction
{
    private Register reg;

    public Print(Register register)
    {
        this.reg = register;
    }

    public bool execute(Resources resources)
    {
        if (reg.isInstruction)
        {
            Console.WriteLine(reg.getInstruction());
        }
        else
        {
            Console.WriteLine(reg.getValue());
        }
        return true;
    }
}