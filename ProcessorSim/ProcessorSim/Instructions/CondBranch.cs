using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class CondBranch : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register newAddress;

    public CondBranch(Register flag, Register newAddress)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(flag);
        this.newAddress = newAddress;
        this.executionType = ExecutionTypes.Branch;
    }

    public bool execute(Resources resources, List<int> args)
    {
        int flagVal = args[0];
        if (flagVal == 1)
        {
            result = newAddress.getValue();
            // resources.reorderBuffer.notifyBranchAddress(reorderBuffer, newAddress.getValue());
            return true;
        }

        result = -1;

        return false; // Return true only if pipeline needs flushing
    }
}