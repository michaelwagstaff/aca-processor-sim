using ProcessorSim.Instructions;
using ProcessorSim.Enums;
namespace ProcessorSim.HardwareResources;

public class ReservationQueueSlot
{
    public Instruction Op;
    public (ExecutionTypes, int)? Q;
    public int V;
    public (ExecutionTypes, int)? AQ;
    public int AV;
    public bool Busy;
    public (ExecutionTypes, int) number;
    public Resources resources;
    public bool ready;
    private bool dispatched;

    public ReservationQueueSlot((ExecutionTypes, int) number, Instruction instruction, Resources resources)
    {
        this.number = number;
        this.resources = resources;
        // Modify to account for Aq and Av
        if (instruction.GetType() == typeof(StoreR))
        {
            if (resources.registerFile.getDependantStation(instruction.inputRegisters[0]) != null)
            {
                Q = ((ExecutionTypes, int))resources.registerFile.getDependantStation(instruction.inputRegisters[0]);
                ready = false;
            }
            else
            {
                V = instruction.inputRegisters[0].getValue();
                ready = true;
            }
        }

        if (instruction.GetType() == typeof(Load) || instruction.GetType() == typeof(LoadI) ||
            instruction.GetType() == typeof(LoadR))
        {
            resources.registerFile.setDependantStation(instruction.targetRegister, number);
        }

        if (instruction.GetType() == typeof(LoadR) || instruction.GetType() == typeof(StoreR))
        {
            RegisterLoadStore temp = (RegisterLoadStore) instruction;
            if (resources.registerFile.getDependantStation(temp.memoryIndexRegister) != null)
            {
                AQ = ((ExecutionTypes, int))resources.registerFile.getDependantStation(temp.memoryIndexRegister);
            }
            else
            {
                AV = temp.memoryIndexRegister.getValue();
            }
        }
        else if(instruction.GetType() == typeof(Load) || instruction.GetType() == typeof(Store))
        {
            ImmediateMemoryLoadStore temp = (ImmediateMemoryLoadStore) instruction;
            AV = temp.memoryIndex;
        }
        Busy = true;
        dispatched = false;
    }

    public void CDBupdate((ExecutionTypes, int) station, int value)
    {
        if (station == Q)
        {
            Q = null;
            V = value;
        }
        if (station == AQ)
        {
            AQ = null;
            AV = value;
        }

        if (station == number && dispatched)
        {
            Busy = false;
            dispatched = false;
        }
        if (AQ == null && Q == null)
            ready = true;
    }

    public (Instruction, List<int>) getInstructionForExecution()
    {
        List<int> returnV = new List<int>();
        returnV.Add(AV);
        returnV.Add(V);
        Op = null;
        this.Q = null;
        this.AQ = null;
        this.V = -1;
        this.AV = -1;
        dispatched = true;
        return (Op, returnV);
    }
}