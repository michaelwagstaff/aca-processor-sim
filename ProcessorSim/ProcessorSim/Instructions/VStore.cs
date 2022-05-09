using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public class VStore : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public VRegister targetRegister { get; set; }
    public int[] result { get; set; }
    public int registerFile { get; set; }
    public List<VRegister> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private VRegister reg;
    public int memoryIndex { get; set; }
    public VStore(VRegister register, int memoryIndex)
    {
        inputRegisters = new List<VRegister>();
        this.inputRegisters.Add(register);
        this.memoryIndex = memoryIndex;
        this.executionType = ExecutionTypes.LoadStore;
    }
    public bool execute(Resources resources, List<int[]> args)
    {
        Instruction instruction = (Instruction) this;
        result = args[1];
        return true;
    }
}