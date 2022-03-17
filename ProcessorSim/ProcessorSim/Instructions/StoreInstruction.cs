namespace ProcessorSim.Instructions;

public interface StoreInstruction : Instruction
{
    public int memoryIndex { get; set; }
}