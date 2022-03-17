using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStationSlot
{
    public bool isEmpty;
    public (Instruction, List<Register>) instructionObject;
    public bool isUnblocked;

    public ReservationStationSlot()
    {
        this.instructionObject = (null, null);
        isEmpty = true;
    }
    public bool addItem((Instruction, List<Register>) instruction)
    {
        if (instruction != (null, null))
        {
            this.instructionObject = instruction;
            if (instruction.Item2.Count == 0)
                isUnblocked = true;
            else
                isUnblocked = false;
            this.isEmpty = false;
        }
        return true;
    }

    public Instruction removeItem()
    {
        Instruction instruction = this.instructionObject.Item1;
        this.instructionObject = (null, null);
        this.isEmpty = true;
        return instruction;
    }

    public bool hasType(ExecutionTypes executionType)
    {
        if (!this.isEmpty)
            return instructionObject.Item1.executionType == executionType;
        return false;
    }
}