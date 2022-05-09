using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadR : RegisterLoadStore
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public Register memoryIndexRegister { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }

    public LoadR(Register register, Register memoryIndexRegister)
    {
        inputRegisters = new List<Register>();
        targetRegister = register;
        this.memoryIndexRegister = memoryIndexRegister;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int> args)
    {
        result = resources.dataMemory[args[0]].getValue();
        return true;
    }
}