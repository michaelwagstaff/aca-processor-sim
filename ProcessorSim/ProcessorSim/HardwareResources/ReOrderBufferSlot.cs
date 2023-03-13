using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBufferSlot
{
    public bool busy;
    public Instruction instruction;
    public ReOrderBufferState state;
    public Register destination;
    public int memoryIndex;
    public int value;
    public int[] vectorValues;

    public void addItem(Instruction instruction)
    {
        busy = true;
        this.instruction = instruction;
        state = ReOrderBufferState.Execute;
        destination = instruction.targetRegister;
        value = -1;

    }
}