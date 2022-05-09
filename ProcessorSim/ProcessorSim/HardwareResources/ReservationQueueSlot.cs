using ProcessorSim.Instructions;
using ProcessorSim.Enums;
namespace ProcessorSim.HardwareResources;

public class ReservationQueueSlot
{
    public Instruction Op;
    public int? Q;
    public int? V;
    public int? AddressQ;
    public int? AddressV;
    public bool Busy;
    public Resources resources;
    public bool ready;

    public ReservationQueueSlot(Instruction instruction, Resources resources)
    {
        ready = true;
        Op = instruction;
        this.resources = resources;
        // Modify to account for Aq and Av
        if (instruction.GetType() == typeof(LoadR) || instruction.GetType() == typeof(StoreR))
        {
            RegisterLoadStore temp = (RegisterLoadStore) instruction;
            if (resources.reorderBuffer.getROBDependency(temp.memoryIndexRegister) != -1)
            {
                int possibleDependency = resources.reorderBuffer.getROBDependency(temp.memoryIndexRegister);
                if (resources.reorderBuffer.getValue(possibleDependency) == null)
                {
                    AddressQ = possibleDependency;
                    ready = false;
                }
                else
                {
                    AddressV = resources.reorderBuffer.getValue(possibleDependency);
                }
            }
            else
            {
                AddressV = temp.memoryIndexRegister.getValue();
            }
        }
        if (instruction.GetType() == typeof(StoreR) || instruction.GetType() == typeof(Store))
        {
            if (resources.reorderBuffer.getROBDependency(instruction.inputRegisters[0]) != -1)
            {
                int possibleDependency = resources.reorderBuffer.getROBDependency(instruction.inputRegisters[0]);
                if (resources.reorderBuffer.getValue(possibleDependency) == null)
                {
                    Q = possibleDependency;
                    ready = false;
                }
                else
                {
                    V = resources.reorderBuffer.getValue(possibleDependency);
                }
            }
            else
            {
                V = instruction.inputRegisters[0].getValue();
            }
        }
        if(instruction.GetType() == typeof(Store))
        {
            ImmediateMemoryLoadStore temp = (ImmediateMemoryLoadStore) instruction;
            AddressV = temp.memoryIndex;
        }
        Busy = true;
    }

    public void CDBupdate(int slot, int value)
    {
        if (slot == Q)
        {
            Q = null;
            V = value;
        }
        if (slot == AddressQ)
        {
            AddressQ = null;
            AddressV = value;
        }
        if (AddressQ == null && Q == null)
            ready = true;
    }

    public (Instruction, List<int>) getInstructionForExecution()
    {
        List<int> returnV = new List<int>();
        if(AddressV != null)
            returnV.Add((int)AddressV);
        else
            returnV.Add(-1);
        if(V != null)
            returnV.Add((int)V);
        else
            returnV.Add(-1);
        return (Op, returnV);
    }
}