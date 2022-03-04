using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Branch : Instruction
{
    public ExecutionTypes executionType { get; set; }
    private Register newAddress;

    public Branch(Register newAddress)
    {
        this.newAddress = newAddress;
        this.executionType = ExecutionTypes.Branch;
    }

    public bool execute(Resources resources)
    {
        Console.WriteLine("Branching");
        resources.pc.setValue(newAddress.getValue() - 1);
        return true;
    }
}