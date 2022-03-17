using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class StoreR : StoreInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private Register memoryIndexReg;
    public int memoryIndex { get; set; }
    public StoreR(Register register, Register memoryIndex)
    {
        this.reg = register;
        this.memoryIndexReg = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        result = instruction.getVal(resources, reg);
        memoryIndex = instruction.getVal(resources, memoryIndexReg);
        return true;
    }
}