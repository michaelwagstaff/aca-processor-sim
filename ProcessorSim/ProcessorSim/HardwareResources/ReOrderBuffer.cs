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
        for (int i = 0; i < frontOfQueue + currentSize; i++)
        {
            if (internalQueue[i].instruction.executionType == ExecutionTypes.Branch)
            {
                return true;
            }
        }
        return false;
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
        internalQueue[slot] = null;
        frontOfQueue++;
        currentSize--;
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