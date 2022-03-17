using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class CondBranch : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register flag;
    private Register newAddress;

    public CondBranch(Register flag, Register newAddress)
    {
        this.flag = flag;
        this.newAddress = newAddress;
        this.executionType = ExecutionTypes.Branch;
    }

    public bool execute(Resources resources)
    {
        if (this.flag.getValue() == 1)
        {
            resources.pc.setValue(newAddress.getValue() - 1);
            return true;
        }

        return false; // Return true only if pipeline needs flushing
    }
}