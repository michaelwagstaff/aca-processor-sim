namespace ProcessorSim.HardwareResources;

public class MemorySlot
{
    private byte[] value;

    public MemorySlot()
    {
        value = new byte[4];
    }

    public bool setValue(byte[] value)
    {
        this.value = value;
        return true;
    }
    public byte[] getValue()
    {
        return this.value;
    }
}