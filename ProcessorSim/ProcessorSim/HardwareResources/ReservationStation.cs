using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStation
{
    private ExecutionTypes executionType;
    private ReservationStationSlot[] internalArray;
    private int emptySlots;
    private int size;
    public ReservationStation(ExecutionTypes executionType, int items)
    {
        this.executionType = executionType;
        internalArray = new ReservationStationSlot[items];
        emptySlots = items;
        size = items;
        for (int i = 0; i < items; i++)
        {
            internalArray[i] = new ReservationStationSlot();
        }
    }

    public bool hasSpace()
    {
        return this.emptySlots != 0;
    }

    public bool addItem((Instruction, Dictionary<Register, int>) instructionObject)
    {
        if (hasSpace())
        {
            for (int i = 0; i < size; i++)
            {
                if (this.internalArray[i].isEmpty)
                {
                    this.internalArray[i].addItem(instructionObject);
                    return true;
                }
            }
        }
        return false;
    }

    public Instruction getItem(ExecutionTypes executionType)
    {
        for (int i = 0; i < size; i++)
        {
            if (this.internalArray[i].hasType(executionType) && this.internalArray[i].isUnblocked)
            {
                if(executionType != ExecutionTypes.Branch)
                    return this.internalArray[i].removeItem();
                else
                {
                    if (onlyBranches())
                    {
                        return this.internalArray[i].removeItem();
                    }
                }
            }
        }

        return new Blank();
    }

    private bool onlyBranches()
    {
        for (int i = 0; i < size; i++)
        {
            if (!this.internalArray[i].hasType(ExecutionTypes.Branch) && !this.internalArray[i].isEmpty)
            {
                return false;
            }
        }
        return true;
    }

    public bool hasType(ExecutionTypes executionType)
    {
        for (int i = 0; i < size; i++)
        {
            if (this.internalArray[i].hasType(executionType))
            {
                return true;
            }
        }

        return false;
    }

    public bool markRegisterUnblocked(Register register)
    {
        for (int i = 0; i < size; i++)
        {
            if (!this.internalArray[i].isEmpty && !this.internalArray[i].isUnblocked)
            {
                this.internalArray[i].decrementRegisterCount(register);
            }
        }
        return true;
    }

    public void printContents()
    {
        for (int i = 0; i < internalArray.Length; i++)
        {
            ReservationStationSlot slot = internalArray[i];
            if (!slot.isEmpty)
            {
                Console.WriteLine("    Slot {0}: {1}, blocked: {2}", i, slot.instructionObject.Item1, !slot.isUnblocked);
                Dictionary<Register, int> registerDict = slot.getRegisterDict();
                foreach (KeyValuePair<Register, int> entry in registerDict)
                {
                    Console.WriteLine("      Register {0} Count {1}", entry.Key.index, entry.Value);
                }
            }
        }
    }

    public bool flush(Resources resources)
    {
        // Used for pipeline flush
        // Currently v naïve as we do not have out of order
        for (int i = 0; i < size; i++)
        {
            this.internalArray[i].removeItem();
        }
        resources.instructionsWaitingDecode.Clear();
        return true;
    }
    
}