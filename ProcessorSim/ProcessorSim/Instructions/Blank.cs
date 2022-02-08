using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Blank : Instruction
{
    public Blank() {}

    public bool execute(Resources resources)
    {
        return true;
    }
}