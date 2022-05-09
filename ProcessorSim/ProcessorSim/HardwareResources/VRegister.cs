using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class VRegister
{
    private int[] value;
    public VRegister()
    {
        value = new int[4];
    }

    public bool setValue(int[] value)
    {
        
        this.value = value;
        return true;
    }
    public int[] getValue()
    {
        return value;
    }
}