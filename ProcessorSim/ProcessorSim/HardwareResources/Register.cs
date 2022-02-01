namespace ProcessorSim.HardwareResources;

public class Register
{
    private byte[] value;

    public Register()
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