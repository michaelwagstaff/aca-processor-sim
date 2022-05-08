using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBufferSlot
{
    private bool busy;
    private Instruction instruction;
    private string state;
    private Register destination;
    private int value;
}