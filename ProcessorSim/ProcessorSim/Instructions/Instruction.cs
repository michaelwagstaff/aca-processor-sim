namespace ProcessorSim.Instructions;

public abstract class Instruction
{
    public bool execute(Resources resources)
    {
        return true;
    }
}