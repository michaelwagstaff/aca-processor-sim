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
        if (instructionObject.Value.Item1 == null)
        {
            if (blocked)
            {
                instruction = currentInstruction;
                args = currentArgs;
            }
            else
            {
                instruction = new Blank();
                args = new List<int>();
            }
        }
        else
        {
            instruction = instructionObject.Value.Item1;
            args = instructionObject.Value.Item2;
        }
        if (resources.verbose)
            Console.WriteLine("  Executing Instruction: {0}", instruction);
        bool? result = null;

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
            /*
            if (instruction.executionType != ExecutionTypes.ComplexArithmetic)
                resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
            */
            if (instruction.executionType == ExecutionTypes.LoadStore && instruction.targetRegister == null)
            {
                resources.CDBBroadcastMemoryAddress(instruction.reorderBuffer,
                        ((StoreInstruction) instruction).memoryIndex);
                resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
            }
            else
                resources.instructionsWaitingMemory.Add(instruction);
            // Console.WriteLine(instruction.ToString());
        }
        else
        {
            if(resources.reorderBuffer.getInstructionForSlot(instruction.reorderBuffer) != null && 
               resources.reorderBuffer.getInstructionForSlot(instruction.reorderBuffer).GetType() == typeof(Blank))
            {
                // Depending on where the nop is generated, there may be an entry for it in the reorder buffer
                resources.CDBBroadcast(instruction.reorderBuffer, instruction.result);
            }
        }

        return result;
    }
}