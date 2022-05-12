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
    private int newAddress;
    public bool predictedTaken;
    public int backupAddress;
    public int originalAddress;

    public CondBranch(Register flag, int newAddress, (bool, int, int) branchDetails)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(flag);
        this.newAddress = newAddress;
        this.executionType = ExecutionTypes.Branch;
        predictedTaken = branchDetails.Item1;
        backupAddress = branchDetails.Item2;
        originalAddress = branchDetails.Item3;
    }

    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        int flagVal = args[0];
        if (flagVal == 1)
        {
            resources.branchPredictor.noteResult(originalAddress, newAddress - 1);
            if (predictedTaken)
            {
                return false;
            }
            else
            {
                resources.pc.setValue(newAddress - 1);
                resources.reorderBuffer.notifyBranchAddress(reorderBuffer, newAddress);
                return true;
            }
        }
        else
        {
            resources.branchPredictor.noteResult(originalAddress, backupAddress);
            if (predictedTaken)
            {
                resources.pc.setValue(backupAddress);
                resources.reorderBuffer.notifyBranchAddress(reorderBuffer, backupAddress + 1);
                return true;
            }
        }
        return false; // Return true only if pipeline needs flushing
    }
}