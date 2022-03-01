using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class MarkAvailable : Instruction
{
    private Register reg;

    public MarkAvailable(Register register)
    {
        this.reg = register;
        this.executionType = ExecutionTypes.General;
    }

    public bool execute(Resources resources)
    {
        this.reg.available = true;
        return true;
    }
}