using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Blank : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public Blank()
    {
        this.executionType = ExecutionTypes.General;
    }

    public bool execute(Resources resources)
    {
        return true;
    }
}