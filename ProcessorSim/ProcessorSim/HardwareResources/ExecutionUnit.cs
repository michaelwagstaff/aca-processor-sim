using System.Reflection;
using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ExecutionUnit
{
    private int cyclesToCompletion;
    public ExecutionTypes type;
    public bool blocked;
    private Instruction currentInstruction;

    public ExecutionUnit(ExecutionTypes type)
    {
        this.type = type;
        blocked = false;
    }
    public bool? execute(Resources resources, Instruction instruction=null)
    {
        if (instruction == null && blocked)
            instruction = currentInstruction;
        bool? result = null;
        if (resources.verbose)
            Console.WriteLine("  Executing Instruction: {0}", instruction);
        if(instruction.GetType().Name != "Blank")
            resources.monitor.incrementInsructionsExecuted();
        if (instruction.executionType == ExecutionTypes.Branch)
            result = instruction.execute(resources);
        else if (instruction.executionType == ExecutionTypes.ComplexArithmetic)
        {
            if (blocked)
            {
                cyclesToCompletion -= 1;
                if (cyclesToCompletion == 0)
                {
                    blocked = false;
                    instruction.execute(resources);
                }
            }
            else
            {
                cyclesToCompletion = 1;
                blocked = true;
                currentInstruction = instruction;
            }
        }
        else
            instruction.execute(resources);
        if (instruction.GetType().Name != "Blank")
        {
            if (instruction.GetType().Name == "ComplexArithmetic")
                resources.instructionsWaitingWriteback.Add(instruction);
            else
                resources.instructionsWaitingMemory.Add(instruction);
        }

        return result;
    }
}