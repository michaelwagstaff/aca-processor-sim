using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Branch : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private int newAddress;

    public Branch(int newAddress)
    {
        inputRegisters = new List<Register>();
        this.newAddress = newAddress;
        this.executionType = ExecutionTypes.Branch;
    }

    public bool execute(Resources resources, List<int> args)
    {
        result = -1;
        return true;
    }
}