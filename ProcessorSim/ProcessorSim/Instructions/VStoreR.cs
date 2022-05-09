using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class VStoreR : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public VRegister targetRegister { get; set; }
    public int[] result { get; set; }
    public int registerFile { get; set; }
    public List<VRegister> inputRegisters { get; set; }
    public Register memoryIndexRegister { get; set; }
    public int reorderBuffer { get; set; }
    public int memoryIndex { get; set; }
    public VStoreR(VRegister register, Register memoryIndex)
    {
        inputRegisters = new List<VRegister>();
        inputRegisters.Add(register);
        this.memoryIndexRegister = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int[]> args)
    {
        result = args[1];
        memoryIndex = args[0][0];
        return true;
    }
}