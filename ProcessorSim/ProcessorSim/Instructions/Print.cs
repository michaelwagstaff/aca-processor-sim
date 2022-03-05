using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Print : Instruction
{
    public ExecutionTypes executionType { get; set; }
    private Register reg;

    public Print(Register register)
    {
        this.reg = register;
        this.executionType = ExecutionTypes.General;
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