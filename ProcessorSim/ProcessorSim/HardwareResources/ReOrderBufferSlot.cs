using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBufferSlot
{
    public bool busy;
    private Instruction instruction;
    private ReOrderBufferState state;
    private Register destination;
    private int value;

    public void addItem(Instruction instruction)
    {
        busy = true;
        this.instruction = instruction;
        this.state = ReOrderBufferState.Execute;
        this.destination = instruction.targetRegister;
    }
}