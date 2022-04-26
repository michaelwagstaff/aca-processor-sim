using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class MarkAvailable : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public (ExecutionTypes, int) reservationStation { get; set; }
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