using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStation
{
    private ExecutionTypes executionType;
    private ReservationStationSlot[] internalArray;
    private Queue<ReservationQueueSlot> ReservationQueue;
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
        if (executionType != ExecutionTypes.LoadStore)
        {
            for (int i = 0; i < items; i++)
            {
                internalArray[i] = new ReservationStationSlot((executionType, i), resources);
            }
        }
        else
        {
            ReservationQueue = new Queue<ReservationQueueSlot>();
        }
    }

    public bool hasSpace()
    {
        if (executionType != ExecutionTypes.LoadStore)
        {
            return this.emptySlots != 0;
        }
        return true;
    }

    public bool addItem(Instruction instruction)
    {
        if (hasSpace() && executionType != ExecutionTypes.LoadStore)
        {
            for (int i = 0; i < size; i++)
            {
                if (this.internalArray[i].Busy == false)
                {
                    this.internalArray[i].addItem(instruction);
                    this.emptySlots --;
                    return true;
                }
            }
        }
        else if (executionType == ExecutionTypes.LoadStore)
        {
            ReservationQueueSlot newSlot =
                new ReservationQueueSlot(instruction, resources);
            size++;
            this.ReservationQueue.Enqueue(newSlot);
            return true;
        }
        return false;
    }

    public (Instruction, List<int>) getItem()
    {
        if (executionType != ExecutionTypes.LoadStore)
        {
            for (int i = 0; i < size; i++)
            {
                if (internalArray[i].ready && internalArray[i].Busy && !internalArray[i].dispatched)
                {
                    this.emptySlots++;
                    return this.internalArray[i].getInstructionForExecution();
                }
            }
        }
        else
        {
            if(ReservationQueue.Count == 0 || ReservationQueue.Peek().ready != true)
                return (new Blank(), new List<int>());
            ReservationQueueSlot slotToReturn = ReservationQueue.Dequeue();
            return slotToReturn.getInstructionForExecution();
        }

        return (new Blank(), new List<int>());
    }

    public void CDBUpdate(int bufferSlot, int value)
    {
        if (executionType != ExecutionTypes.LoadStore)
        {
            for (int i = 0; i < size; i++)
            {
                if (this.internalArray[i].Busy)
                {
                    this.internalArray[i].CDBupdate(bufferSlot, value);
                }
            }
        }
        else
        {
            foreach (ReservationQueueSlot queuedItem in ReservationQueue)
            {
                queuedItem.CDBupdate(bufferSlot, value);
            }
        }
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