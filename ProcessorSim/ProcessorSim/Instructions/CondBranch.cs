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
    private Register flag;
    private Register newAddress;

    public CondBranch(Register flag, Register newAddress)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(flag);
        this.flag = flag;
        this.newAddress = newAddress;
        this.executionType = ExecutionTypes.Branch;
    }

    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        int flagVal = args[0];
        if (flagVal == 1)
        {
            resources.pc.setValue(newAddress.getValue() - 1);
            resources.reorderBuffer.notifyBranchAddress(reorderBuffer, newAddress.getValue());
            return true;
        }

        return false; // Return true only if pipeline needs flushing
    }
}