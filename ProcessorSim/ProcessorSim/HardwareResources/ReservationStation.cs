using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReservationStation
{
    private ExecutionTypes executionType;
    private List<ReservationStationSlot> internalArray;
    private Queue<ReservationQueueSlot> ReservationQueue;
    private int emptySlots;
    private int size;
    private Resources resources;
    public ReservationStation(ExecutionTypes executionType, int items, Resources resources)
    {
        this.executionType = executionType;
        internalArray = new List<ReservationStationSlot>();
        emptySlots = items;
        size = items;
        this.resources = resources;
        if (executionType == ExecutionTypes.LoadStore || executionType == ExecutionTypes.Vector)
            ReservationQueue = new Queue<ReservationQueueSlot>();
    }

    public bool hasSpace()
    {
        if (executionType != ExecutionTypes.LoadStore && executionType != ExecutionTypes.Vector)
        {
            return this.emptySlots != 0;
        }
        return true;
    }

    public bool addItem(Instruction instruction)
    {
        if (hasSpace())
        {
            if (executionType != ExecutionTypes.LoadStore && executionType != ExecutionTypes.Vector)
            {
                ReservationStationSlot newSlot = new ReservationStationSlot(resources);
                newSlot.addItem(instruction);
                internalArray.Add(newSlot);
                this.emptySlots--;
                return true;
            }
            else
            {
                ReservationQueueSlot newSlot =
                    new ReservationQueueSlot(instruction, resources);
                size++;
                this.ReservationQueue.Enqueue(newSlot);
                return true;
            }
        }

        return false;
    }

    public (Instruction, List<int>) getItem()
    {
        if (executionType != ExecutionTypes.LoadStore && executionType != ExecutionTypes.Vector)
        {
            for (int i = 0; i < size-emptySlots; i++)
            {
                internalArray[i].CDBupdate(-1,-1);
                if (internalArray[i].ready && !internalArray[i].dispatched)
                {
                    this.emptySlots++;
                    (Instruction, List<int>) returnVal = this.internalArray[i].getInstructionForExecution();
                    internalArray.RemoveAt(i);
                    return returnVal;
                }
            }
        }
        else
        {
            if(ReservationQueue.Count == 0 || ReservationQueue.Peek().ready != true)
                return (new Blank(), new List<int>());
            ReservationQueueSlot slotToReturn = ReservationQueue.Dequeue();
            Console.WriteLine(slotToReturn.getInstructionForExecution().ToString());
            return slotToReturn.getInstructionForExecution();
        }

        return (new Blank(), new List<int>());
    }

    public void CDBUpdate(int bufferSlot, int value)
    {
        if (executionType != ExecutionTypes.LoadStore && executionType != ExecutionTypes.Vector)
        {
            for (int i = 0; i < size - emptySlots; i++)
            {
                if (internalArray[i] != null && this.internalArray[i].Busy)
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
    public void CDBUpdate(int bufferSlot, int[] value)
    {
        foreach (ReservationQueueSlot queuedItem in ReservationQueue)
        {
            queuedItem.CDBupdate(bufferSlot, value);
        }
    }

    public void flush(int branchSlot)
    {
        if (executionType != ExecutionTypes.LoadStore)
        {
            List<ReservationStationSlot> tempNewArray = new List<ReservationStationSlot>();
            foreach (ReservationStationSlot slot in internalArray)
            {
                emptySlots = size;
                if (slot.Op.reorderBuffer < branchSlot)
                {
                    // It stays!
                    tempNewArray.Add(slot);
                    emptySlots--;
                }
                else
                {
                    // It's culled :(
                }
            }
            internalArray = tempNewArray;
        }
        else
        {
            Queue<ReservationQueueSlot> tempReservationQueue = new Queue<ReservationQueueSlot>();
            foreach (ReservationQueueSlot slot in ReservationQueue.ToList())
            {
                emptySlots = size;
                if (slot.Op.reorderBuffer < branchSlot)
                {
                    // It stays!
                    tempReservationQueue.Enqueue(slot);
                    emptySlots--;
                }
                else
                {
                    // It's culled :(
                }
            }
            ReservationQueue = tempReservationQueue;
        }
    }
    
    public void printContents()
    {
        for (int i = 0; i < internalArray.Count; i++)
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