using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.HardwareResources;

public class ReservationStationSlot
{
    public Instruction Op;
    public List<int?> Q;
    public List<int?> V;
    public bool Busy;
    public (ExecutionTypes, int) number;
    public Resources resources;
    public int destination;
    public bool ready;
    public bool dispatched;

    public ReservationStationSlot(Resources resources)
    {
        this.number = number;
        this.resources = resources;
        Busy = false;
        ready = false;
        dispatched = false;
        this.Q = new List<int?>();
        this.V = new List<int?>();
    }
    public bool addItem(Instruction instruction)
    {
        if (instruction == null)
            return false;
        Op = instruction;
        ready = true;
        for (int i = 0; i < instruction.inputRegisters.Count; i++)
        {
            Register inputRegister = instruction.inputRegisters[i];
            if (resources.reorderBuffer.getROBDependency(inputRegister) != -1)
            {
                int possibleDependency = resources.reorderBuffer.getROBDependency(inputRegister);
                if (resources.reorderBuffer.getValue(possibleDependency) == null || resources.reorderBuffer.getValue(possibleDependency) == -1)
                {
                    Q.Add(possibleDependency);
                    V.Add(null);
                    ready = false;
                }
                else
                {
                    Q.Add(null);
                    V.Add(resources.reorderBuffer.getValue(possibleDependency));
                }
            }
            else
            {
                Q.Add(null);
                V.Add(instruction.inputRegisters[i].getValue());
            }
        }
        if (Op.GetType() == typeof(Print))
        {
            Console.Write("");
        }
        Busy = true;
        return true;
    }

    public void CDBupdate(int bufferSlot, int value)
    {
        for (int i = 0; i < Q.Count; i++)
        {
            if (bufferSlot == Q[i])
            {
                Q[i] = null;
                V[i] = value;
            }
        }
        if(Busy && Op != null)
            ready = true;
        for (int i = 0; i < Q.Count; i++)
        {
            if (Q[i] != null)
            {
                ready = false;
            }
        }
        if (Op.GetType() == typeof(Copy))
        {
            Console.Write("");
        }

        if (Op.GetType() == typeof(Print))
        {
            Console.Write("");
        }

        if (Op.GetType() == typeof(Branch) || Op.GetType() == typeof(CondBranch))
        {
            if (!resources.reorderBuffer.allPreviousSlotsExecuted(Op.reorderBuffer))
                ready = false;
        }
        else if (!resources.reorderBuffer.allPreviousBranchesExecuted(Op.reorderBuffer))
                ready = false;
    }

    public (Instruction, List<int>) getInstructionForExecution()
    {
        List<int> returnV = new List<int>(V.Where(x => x != null).Cast<int>().ToList());
        if (Op.GetType() == typeof(Copy))
        {
            Console.Write("");
        }
        Busy = false;
        return (Op, returnV);
    }
}