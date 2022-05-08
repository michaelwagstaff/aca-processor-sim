using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBuffer
{
    private ReOrderBufferSlot[] internalQueue;
    private int frontOfQueue;
    private int maxQueueSize;
    private int currentSize;

    public ReOrderBuffer()
    {
        maxQueueSize = 64;
        internalQueue = new ReOrderBufferSlot[maxQueueSize];
        frontOfQueue = 0;
        currentSize = 0;
    }

    public bool hasFreeSlot()
    {
        int index = frontOfQueue - 1;
        if (index == -1)
            index = maxQueueSize - 1;
        return !internalQueue[index].busy;
    }
    public int addItemToBuffer(Instruction instruction)
    {
        ReOrderBufferSlot slot = new ReOrderBufferSlot();
        slot.addItem(instruction);
        internalQueue[frontOfQueue + currentSize] = slot;
        currentSize++;
        return frontOfQueue + currentSize - 1;
    }

    public int getROBDependency(Register register)
    {
        int dependency = -1;
        for (int i = 0; i < currentSize; i++)
        {
            if (internalQueue[frontOfQueue + i].destination == register)
            {
                dependency = i;
            }
        }
        return dependency;
    }

    public int? getValue(int slot)
    {
        if (this.internalQueue[slot].state == ReOrderBufferState.Execute)
            return null;
        return internalQueue[slot].value;
    }
}