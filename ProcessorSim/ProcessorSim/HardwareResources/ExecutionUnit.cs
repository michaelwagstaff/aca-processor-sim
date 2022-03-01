using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ExecutionUnit
{
    private int cyclesToCompletion;
    public ExecutionTypes type;

    public ExecutionUnit(ExecutionTypes type)
    {
        this.type = type;
    }
    public void execute(Resources resources, Instruction instruction)
    {
        instruction.execute(resources);
    }
}