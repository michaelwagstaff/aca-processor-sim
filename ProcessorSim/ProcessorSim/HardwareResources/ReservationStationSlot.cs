using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStationSlot
{
    public bool isEmpty;
    public Instruction instruction;

    public ReservationStationSlot()
    {
        this.instruction = null;
        isEmpty = true;
    }
    public bool addItem(Instruction instruction)
    {
        if (instruction != null)
        {
            this.instruction = instruction;
            this.isEmpty = false;
        }
        return true;
    }

    public Instruction removeItem()
    {
        Instruction instruction = this.instruction;
        this.instruction = null;
        this.isEmpty = true;
        return instruction;
    }

    public bool hasType(ExecutionTypes executionType)
    {
        if (!this.isEmpty)
            return instruction.executionType == executionType;
        return false;
    }
}