using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Blank : Instruction
{
    public Blank()
    {
        this.executionType = ExecutionTypes.General;
    }

    public bool execute(Resources resources)
    {
        return true;
    }
}