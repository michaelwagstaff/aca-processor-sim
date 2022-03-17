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
        bool? result = null;
        if (resources.verbose)
            Console.WriteLine("Executing Instruction: {0}", instruction);
        if(instruction.GetType().Name != "Blank")
            resources.monitor.incrementInsructionsExecuted();
        if (instruction.executionType == ExecutionTypes.Branch)
            result = instruction.execute(resources);
        else
            instruction.execute(resources);
        if (instruction.targetRegister != null)
            resources.dataHazards[instruction.targetRegister] = true;
        if(instruction.GetType().Name != "Blank")
            resources.instructionWaitingMemory = instruction;
        return result;
    }
}