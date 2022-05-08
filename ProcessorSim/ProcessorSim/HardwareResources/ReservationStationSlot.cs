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

    public ReservationStationSlot((ExecutionTypes, int) number, Resources resources)
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
        Register source1, source2;
        // Assume complex arithmetic
        // Should be source 1
        ready = true;
        for (int i = 0; i < instruction.inputRegisters.Count; i++)
        {
            Register inputRegister = instruction.inputRegisters[i];
            if (resources.reorderBuffer.getROBDependency(inputRegister) != null)
            {
                int possibleDependency = resources.reorderBuffer.getROBDependency(inputRegister);
                if (resources.reorderBuffer.getValue(possibleDependency) == null)
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
    }

    public (Instruction, List<int>) getInstructionForExecution()
    {
        List<int> returnV = new List<int>(V.Where(x => x != null).Cast<int>().ToList());
        Busy = false;
        return (Op, returnV);
    }
}