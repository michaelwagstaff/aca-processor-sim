using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBufferSlot
{
    public bool busy;
    private Instruction instruction;
    public ReOrderBufferState state;
    public Register destination;
    public int memoryIndex;
    public int value;

    public void addItem(Instruction instruction)
    {
        busy = true;
        this.instruction = instruction;
        this.state = ReOrderBufferState.Execute;
        this.destination = instruction.targetRegister;
    }
}