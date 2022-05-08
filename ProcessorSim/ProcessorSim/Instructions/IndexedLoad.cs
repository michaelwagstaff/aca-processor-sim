using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class IndexedLoad : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private Register memoryIndexRegister;
    private Register offset;
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }

    public IndexedLoad(Register register, Register memoryIndexRegister, Register offset)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(memoryIndexRegister);
        inputRegisters.Add(offset);
        this.reg = register;
        this.memoryIndexRegister = memoryIndexRegister;
        this.offset = offset;
        this.executionType = ExecutionTypes.LoadStore;
        this.reg.available = false;
        // Rather important, once decoded, we can't change register, so need to make sure nothing else uses it!
    }
    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        result = resources.dataMemory[args[0] + args[1]].getValue();
        targetRegister = reg;
        return true;
    }
}