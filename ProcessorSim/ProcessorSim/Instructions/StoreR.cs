using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class StoreR : StoreInstruction, RegisterLoadStore
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public (ExecutionTypes, int) reservationStation { get; set; }
    private Register reg;
    public Register memoryIndexRegister { get; set; }
    public int memoryIndex { get; set; }
    public StoreR(Register register, Register memoryIndex)
    {
        this.reg = register;
        this.memoryIndexRegister = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int> args)
    {
        Instruction instruction = (Instruction) this;
        result = args[1];
        memoryIndex = args[0];
        return true;
    }
}