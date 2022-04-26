using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public interface RegisterLoadStore : Instruction
{
    Register memoryIndexRegister { get; set; }
}