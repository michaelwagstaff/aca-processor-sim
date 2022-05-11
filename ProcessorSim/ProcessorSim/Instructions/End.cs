using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class End : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }

    public End()
    {
        inputRegisters = new List<Register>();
        this.executionType = ExecutionTypes.General;
    }

    public bool execute(Resources resources, List<int> args)
    {
        return true;
    }
}