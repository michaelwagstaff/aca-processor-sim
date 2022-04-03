using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStationSlot
{
    public bool isEmpty;
    public (Instruction, Dictionary<Register, int>) instructionObject;
    public bool isUnblocked;

    public ReservationStationSlot()
    {
        this.instructionObject = (null, null);
        isEmpty = true;
    }
    public bool addItem((Instruction, Dictionary<Register, int>) instruction)
    {
        if (instruction != (null, null))
        {
            this.instructionObject = instruction;
            isUnblocked = true;
            updateUnblocked();
            this.isEmpty = false;
        }
        return true;
    }

    public Dictionary<Register, int> getRegisterDict()
    {
        return instructionObject.Item2;
    }
    public void updateUnblocked()
    {
        isUnblocked = true;
        foreach (KeyValuePair<Register, int> registerEntry in instructionObject.Item2)
        {
            if (registerEntry.Value > 0)
            {
                isUnblocked = false;
                // Console.WriteLine("Blocked by register {0}", registerEntry.Key.index);
            }
        }
    }

    public Instruction removeItem()
    {
        Instruction instruction = this.instructionObject.Item1;
        this.instructionObject = (null, null);
        this.isEmpty = true;
        this.isUnblocked = true;
        return instruction;
    }

    public bool hasType(ExecutionTypes executionType)
    {
        if (!this.isEmpty)
            return instructionObject.Item1.executionType == executionType;
        return false;
    }

    public void decrementRegisterCount(Register register)
    {
        if (instructionObject.Item2.Keys.Contains(register))
        {
            instructionObject.Item2[register]--;
            updateUnblocked();
        }
    }
}