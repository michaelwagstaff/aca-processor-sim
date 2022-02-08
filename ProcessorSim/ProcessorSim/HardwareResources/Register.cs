namespace ProcessorSim.HardwareResources;

public class Register
{
    private int value;
    public bool available;
    public Register()
    {
        available = true;
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