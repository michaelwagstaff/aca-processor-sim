using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStation
{
    private ExecutionTypes executionType;
    private ReservationStationSlot[] internalArray;
    private int emptySlots;
    private int size;
    private Resources resources;
    public ReservationStation(ExecutionTypes executionType, int items, Resources resources)
    {
        this.executionType = executionType;
        internalArray = new ReservationStationSlot[items];
        emptySlots = items;
        size = items;
        this.resources = resources;
        for (int i = 0; i < items; i++)
        {
            internalArray[i] = new ReservationStationSlot((executionType, i), resources);
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
                if (this.internalArray[i].Op == null)
                {
                    this.internalArray[i].addItem(instruction);
                    this.emptySlots --;
                    return true;
                }
            }
        }
        return false;
    }

    public (Instruction, List<int>) getItem()
    {
        for (int i = 0; i < size; i++)
        {
            if (this.internalArray[i].ready)
            {
                this.emptySlots++;
                return this.internalArray[i].getInstructionForExecution();
            }
        }
        return (new Blank(), new List<int>());
    }
    public void printContents()
    {
        for (int i = 0; i < internalArray.Length; i++)
        {
            ReservationStationSlot slot = internalArray[i];
            if (slot.Busy)
            {
                Console.WriteLine("    Slot {0}: {1}, blocked: {2}", i, slot.number, !slot.ready);
                /*
                Dictionary<Register, int> registerDict = slot.getRegisterDict();
                foreach (KeyValuePair<Register, int> entry in registerDict)
                {
                    Console.WriteLine("      Register {0} Count {1}", entry.Key.index, entry.Value);
                }
                */
            }
        }
    }
    /*
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
    */
    
}