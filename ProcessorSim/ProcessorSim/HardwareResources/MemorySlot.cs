namespace ProcessorSim.HardwareResources;

public class MemorySlot
{
    private int value;
    private string instruction;

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
    public bool setInstruction(string instruction)
    {
        this.instruction = instruction;
        return true;
    }
    public string getInstruction()
    {
        return this.instruction;
    }
}