namespace ProcessorSim.Instructions;

public interface ImmediateMemoryLoadStore : Instruction
{
    int memoryIndex { get; set; }
}