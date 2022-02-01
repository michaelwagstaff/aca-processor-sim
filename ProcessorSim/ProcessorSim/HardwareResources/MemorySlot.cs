namespace ProcessorSim.HardwareResources;

public class MemorySlot
{
    private int value;

    public MemorySlot()
    {
    }

    public bool setValue(int value)
    {
        this.value = value;
        return true;
    }
    public int getValue()
    {
        return this.value;
    }
}