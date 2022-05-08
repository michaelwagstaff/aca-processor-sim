using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class LoadR : RegisterLoadStore
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    public Register memoryIndexRegister { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }

    public LoadR(Register register, Register memoryIndexRegister)
    {
        inputRegisters = new List<Register>();
        this.reg = register;
        targetRegister = reg;
        this.memoryIndexRegister = memoryIndexRegister;
        this.executionType = ExecutionTypes.LoadStore;
        this.reg.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        result = resources.dataMemory[args[0]].getValue();
        return true;
    }
}