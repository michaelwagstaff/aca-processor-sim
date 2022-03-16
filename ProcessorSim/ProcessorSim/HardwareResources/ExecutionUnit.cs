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
    public bool? execute(Resources resources, Instruction instruction)
    {
        if(instruction.GetType().Name != "Blank")
            resources.monitor.incrementInsructionsExecuted();
        if (instruction.executionType == ExecutionTypes.Branch)
            return instruction.execute(resources);
        else
            instruction.execute(resources);
        return null;
    }
}