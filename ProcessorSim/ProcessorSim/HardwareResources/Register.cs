using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class Register
{
    private int value;
    private string instruction;
    public bool isInstruction;
    public bool available;
    public int index;
    public Register()
    {
        available = true;
    }

    public bool setValue(int value)
    {
        this.value = value;
        this.isInstruction = false;
        return true;
    }
    public int getValue()
    {
        return this.value;
    }

    public string getInstruction()
    {
        return this.instruction;
    }

    public bool setInstruction(string instruction)
    {
        this.instruction = instruction;
        this.isInstruction = true;
        return true;
    }
}