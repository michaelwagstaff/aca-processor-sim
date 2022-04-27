using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Blank : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public (ExecutionTypes, int) reservationStation { get; set; }

    public Blank()
    {
        this.executionType = ExecutionTypes.General;
        this.inputRegisters = new List<Register>();
    }

    public bool execute(Resources resources, List<int> args)
    {
        return true;
    }
}