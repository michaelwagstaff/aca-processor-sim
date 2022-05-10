using System.Linq;
using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class ReOrderBuffer
{
    private ReOrderBufferSlot[] internalQueue;
    private int frontOfQueue;
    private int maxQueueSize;
    private int currentSize;
    private Resources resources;

    public ReOrderBuffer(Resources resources)
    {
        maxQueueSize = 600;
        internalQueue = new ReOrderBufferSlot[maxQueueSize];
        frontOfQueue = 0;
        currentSize = 0;
        this.resources = resources;
    }

    public bool hasFreeSlot()
    {
        int index = frontOfQueue - 1;
        if (index == -1)
            index = maxQueueSize - 1;
        return !internalQueue[index].busy;
    }
    public int addItemToBuffer(Instruction instruction)
    {
        if (frontOfQueue + currentSize > 8)
        {
            Console.Write("");
        }
        ReOrderBufferSlot slot = new ReOrderBufferSlot();
        slot.addItem(instruction);
        internalQueue[frontOfQueue + currentSize] = slot;
        currentSize++;
        return frontOfQueue + currentSize - 1;
    }

    public int getROBDependency(Register register)
    {
        int dependency = -1;
        if (register == null)
            return -1;
        for (int i = 0; i < currentSize - 1; i++)
        {
            // Excludes newly added row
            if (internalQueue[frontOfQueue + i].destination == register)
            {
                dependency = frontOfQueue + i;
            }
        }
        return dependency;
    }

    public int? getValue(int slot)
    {
        if (this.internalQueue[slot].state == ReOrderBufferState.Execute)
            return null;
        return internalQueue[slot].value;
    }

    public void notifyBranchAddress(int slot, int pcValue)
    {
        for (int i = 0; i < resources.decodeUnits.Count; i++)
        {
            resources.registers[31 - i].available = true;
            resources.registers[31 - i].setInstruction("");
        }
        resources.instructionsWaitingDecode = new List<(int, (bool, bool))>();
        foreach (ReservationStation reservationStation in resources.reservationStations.Values)
        {
            reservationStation.flush();
        }

        for (int i = slot + 1; i < frontOfQueue + currentSize; i++)
        {
            internalQueue[i] = null;
            
        }
        currentSize = slot - frontOfQueue + 1;
    }
    public void setMemoryAddress(int slot, int memoryAddress)
    {
        internalQueue[slot].memoryIndex = memoryAddress;
    }

    public void CDBUpdate(int slot, int value)
    {
        internalQueue[slot].value = value;
        internalQueue[slot].state = ReOrderBufferState.WriteResult;
    }

    public bool containsBranch()
    {
        for (int i = frontOfQueue; i < frontOfQueue + currentSize; i++)
        {
            if (internalQueue[i].instruction.executionType == ExecutionTypes.Branch)
            {
                return true;
            }
        }
        return false;
    }

    public bool allPreviousBranchesExecuted(int slot)
    {
        for(int i = frontOfQueue; i < slot; i++)
        {
            if ((internalQueue[i].instruction.GetType() == typeof(Branch)
                || internalQueue[i].instruction.GetType() == typeof(CondBranch))
                && internalQueue[i].state == ReOrderBufferState.Execute)
            {
                return false;
            }
        }
        return true;
    }
    public bool allPreviousSlotsExecuted(int slot)
    {
        for(int i = frontOfQueue; i < slot; i++)
        {
            if (internalQueue[i].state == ReOrderBufferState.Execute)
            {
                return false;
            }
        }
        return true;
    }

    public void commit(int superscalarCount)
    {
        int firstIndexReady = frontOfQueue;
        try
        {
            while (internalQueue[firstIndexReady].state == ReOrderBufferState.Commit)
            {
                firstIndexReady++;
            }
        }
        catch
        {
            return;
        }

        for (int i = 0; i < superscalarCount; i++)
        {
            if (internalQueue[firstIndexReady + i] == null)
                return;
            if (internalQueue[firstIndexReady + i].state == ReOrderBufferState.WriteResult)
            {
                //Commit
                Instruction instruction = internalQueue[firstIndexReady + i].instruction;
                if (instruction.executionType == ExecutionTypes.LoadStore && instruction.targetRegister == null)
                {
                    resources.memoryUnit.memory(resources, instruction);
                }
                else
                {
                    resources.commitUnit.commit(resources, instruction);
                }
            }
            else
            {
                return;
            }
        }
    }

    public void notifyCommitted(int slot)
    {
        if (internalQueue[slot].instruction.GetType().Name != "Blank")
        {
            resources.monitor.incrementInsructionsExecuted();
        }
        if (internalQueue[slot] != null)
        {
            internalQueue[slot] = null;
            frontOfQueue++;
            currentSize--;
        }
    }

    public Instruction getInstructionForSlot(int slot)
    {
        try
        {
            return internalQueue[slot].instruction;
        }
        catch
        {
            return null;
        }
    }
}