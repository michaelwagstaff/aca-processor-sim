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
    private List<int> currentArgs;

    public ExecutionUnit(ExecutionTypes type)
    {
        this.type = type;
        blocked = false;
    }
    public bool? execute(Resources resources, (Instruction, List<int>)? instructionObject)
    {
        Instruction instruction;
        List<int> args;
        if (instructionObject == null && blocked)
        {
            instruction = currentInstruction;
            args = currentArgs;
        }
        else
        {
            instruction = instructionObject.Value.Item1;
            args = instructionObject.Value.Item2;
        }

        bool? result = null;
        if (instruction.GetType().Name != "Blank")
        {
            resources.monitor.incrementInsructionsExecuted();
            if (resources.verbose)
                Console.WriteLine("  Executing Instruction: {0}", instruction);
        }

        if (instruction.executionType == ExecutionTypes.Branch)
            result = instruction.execute(resources, args);
        else if (instruction.executionType == ExecutionTypes.ComplexArithmetic)
        {
            if (blocked)
            {
                cyclesToCompletion -= 1;
                if (cyclesToCompletion == 0)
                {
                    blocked = false;
                    instruction.execute(resources, args);
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
            instruction.execute(resources, args);
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