namespace ProcessorSim.HardwareResources;

public class Register
{
    private int value;

    public Register()
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