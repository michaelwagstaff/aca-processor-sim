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

    public bool addItem(Instruction instruction)
    {
        if (hasSpace())
        {
            for (int i = 0; i < size; i++)
            {
                if (this.internalArray[i].isEmpty)
                {
                    this.internalArray[i].addItem(instruction);
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
            if (this.internalArray[i].hasType(executionType))
            {
                return this.internalArray[i].removeItem();
            }
        }

        return new Blank();
    }

    public bool flush()
    {
        // Used for pipeline flush
        // Currently v naïve as we do not have out of order
        for (int i = 0; i < size; i++)
        {
            this.internalArray[i].removeItem();
        }

        return true;
    }
}