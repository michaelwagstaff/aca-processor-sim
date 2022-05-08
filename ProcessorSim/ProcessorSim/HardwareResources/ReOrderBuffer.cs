using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBuffer
{
    private ReOrderBufferSlot[] internalArray;

    public ReOrderBuffer()
    {
        internalArray = new ReOrderBufferSlot[64];
    }

    public bool hasFreeSlot()
    {
        for (int i = 0; i < internalArray.Length; i++)
        {
            if (!internalArray[i].busy)
                return true;
        }
        return false;
    }

    public int addItemToBuffer(Instruction instruction)
    {
        for (int i = 0; i < internalArray.Length; i++)
        {
            if (!internalArray[i].busy)
            {
                internalArray[i].addItem(instruction);
                return i;
            }
        }
        // If we check hasFreeSlot first (which we should do) then this is unreachable
        return -1;
    }
}