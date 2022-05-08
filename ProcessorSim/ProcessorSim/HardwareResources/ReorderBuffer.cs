namespace ProcessorSim.HardwareResources;

public class ReorderBuffer
{
    private ReOrderBufferSlot[] internalArray;

    public ReorderBuffer()
    {
        internalArray = new ReOrderBufferSlot[32];
    }
}