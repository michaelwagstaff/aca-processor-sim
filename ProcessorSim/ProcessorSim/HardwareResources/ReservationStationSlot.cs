using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.HardwareResources;

public class ReservationStationSlot
{
    public Instruction Op;
    public List<(ExecutionTypes, int)?> Q;
    public List<int?> V;
    public bool Busy;
    public (ExecutionTypes, int) number;
    public Resources resources;
    public bool ready;
    public bool dispatched;

    public ReservationStationSlot((ExecutionTypes, int) number, Resources resources)
    {
        this.number = number;
        this.resources = resources;
        Busy = false;
        ready = false;
        dispatched = false;
        this.Q = new List<(ExecutionTypes, int)?>();
        this.V = new List<int?>();
    }
    public bool addItem(Instruction instruction)
    {
        if (instruction == null)
            return false;
        Register source1, source2;
        // Assume complex arithmetic
        // Should be source 1
        ready = true;
        for (int i = 0; i < instruction.inputRegisters.Count; i++)
        {
            Register inputRegister = instruction.inputRegisters[i];
            if (resources.registerFile.getDependantStation(inputRegister) != null)
            {
                Q.Add(((ExecutionTypes, int)) resources.registerFile.getDependantStation(inputRegister));
                V.Add(null);
                ready = false;
            }
            else
            {
                Q.Add(null);
                V.Add(instruction.inputRegisters[i].getValue());
            }
        }
        Busy = true;
        resources.registerFile.setDependantStation(instruction.targetRegister, number);
        instruction.reservationStation = this.number;
        return true;
    }

    public void CDBupdate((ExecutionTypes, int) station, int value)
    {
        for (int i = 0; i < Q.Count; i++)
        {
            if (station == Q[i])
            {
                Q[i] = null;
                V[i] = value;
            }
        }
        if (station == number && dispatched)
        {
            resetStation();
        }
        if(Op != null)
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
        dispatched = true;
        return (Op, returnV);
    }

    private void resetStation()
    {
        Op = null;
        this.Q = new List<(ExecutionTypes, int)?>();
        this.V = new List<int?>();
        Busy = false;
        dispatched = false;
        ready = false;
    }
}