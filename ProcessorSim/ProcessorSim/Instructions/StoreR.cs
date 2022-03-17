using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class StoreR : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    private Register reg;
    private Register memoryIndex;
    public StoreR(Register register, Register memoryIndex)
    {
        this.reg = register;
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources)
    {
        Instruction instruction = (Instruction) this;
        int regVal = instruction.getVal(resources, reg);
        int indexVal = instruction.getVal(resources, memoryIndex);
        resources.dataMemory[indexVal].setValue(regVal);
        return true;
    }
}