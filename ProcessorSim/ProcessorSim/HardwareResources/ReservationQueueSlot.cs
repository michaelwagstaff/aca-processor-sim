using ProcessorSim.Instructions;
using ProcessorSim.Enums;
namespace ProcessorSim.HardwareResources;

public class ReservationQueueSlot
{
    public Instruction Op;
    public int? Q;
    public int? V;
    public List<int?> Qs;
    public List<int?> Vs;
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
        if (instruction.executionType == ExecutionTypes.Vector)
        {
            Qs = new List<int?>();
            Vs = new List<int?>();
            if (instruction.GetType() == typeof(VAdd) || instruction.GetType() == typeof(VPrint))
            {
                for (int i = 0; i < instruction.inputRegisters.Count; i++)
                {
                    Register inputRegister = instruction.inputRegisters[i];
                    if (resources.reorderBuffer.getROBDependency(inputRegister, instruction.reorderBuffer) != -1)
                    {
                        int possibleDependency = resources.reorderBuffer.getROBDependency(inputRegister, instruction.reorderBuffer);
                        if (resources.reorderBuffer.getVectorValues(possibleDependency) == null || resources.reorderBuffer.getValue(possibleDependency) == -1)
                        {
                            Qs.Add(possibleDependency);
                            for (int j = 0; j < 4; j++)
                            {
                                Vs.Add(null);
                            }

                            ready = false;
                        }
                        else
                        {
                            Qs.Add(null);
                            for (int j = 0; j < 4; j++)
                            {
                                Vs.Add(resources.reorderBuffer.getVectorValues(possibleDependency)[j]);
                            }
                        }
                    }
                    else
                    {
                        Qs.Add(null);
                        for (int j = 0; j < 4; j++)
                        {
                            Vs.Add(instruction.inputRegisters[i].getValueVector()[j]);
                        }
                    }
                }
            }
            else if (instruction.GetType() == typeof(VStoreR) || instruction.GetType() == typeof(VLoadR))
            {
                RegisterLoadStore temp = (RegisterLoadStore) instruction;
                if (resources.reorderBuffer.getROBDependency(temp.memoryIndexRegister, instruction.reorderBuffer) != -1)
                {
                    int possibleDependency = resources.reorderBuffer.getROBDependency(temp.memoryIndexRegister, instruction.reorderBuffer);
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
            if (instruction.GetType() == typeof(VStoreR) || instruction.GetType() == typeof(VStore))
            {
                if (resources.reorderBuffer.getROBDependency(instruction.inputRegisters[0], instruction.reorderBuffer) != -1)
                {
                    int possibleDependency = resources.reorderBuffer.getROBDependency(instruction.inputRegisters[0], instruction.reorderBuffer);
                    if (resources.reorderBuffer.getVectorValues(possibleDependency) == null)
                    {
                        Qs.Add(possibleDependency);
                        for (int j = 0; j < 4; j++)
                        {
                            Vs.Add(null);
                        }
                        ready = false;
                    }
                    else
                    {
                        Qs.Add(null);
                        for (int j = 0; j < 4; j++)
                        {
                            Vs.Add(resources.reorderBuffer.getVectorValues(possibleDependency)[j]);
                        }
                    }
                }
                else
                {
                    Qs.Add(null);
                    for (int j = 0; j < 4; j++)
                    {
                        Vs.Add(instruction.inputRegisters[j].getValueVector()[j]);
                    }
                }
            }

            if (instruction.GetType() == typeof(Store))
            {
                ImmediateMemoryLoadStore temp = (ImmediateMemoryLoadStore) instruction;
                AddressV = temp.memoryIndex;
            }
        }
        else
        {
            if (instruction.GetType() == typeof(LoadR) || instruction.GetType() == typeof(StoreR))
            {
                RegisterLoadStore temp = (RegisterLoadStore) instruction;
                if (resources.reorderBuffer.getROBDependency(temp.memoryIndexRegister, instruction.reorderBuffer) != -1)
                {
                    int possibleDependency = resources.reorderBuffer.getROBDependency(temp.memoryIndexRegister, instruction.reorderBuffer);
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
                if (resources.reorderBuffer.getROBDependency(instruction.inputRegisters[0], instruction.reorderBuffer) != -1)
                {
                    int possibleDependency = resources.reorderBuffer.getROBDependency(instruction.inputRegisters[0], instruction.reorderBuffer);
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

            if (instruction.GetType() == typeof(Store))
            {
                ImmediateMemoryLoadStore temp = (ImmediateMemoryLoadStore) instruction;
                AddressV = temp.memoryIndex;
            }
        }

        Busy = true;
    }

    public void CDBupdate(int slot, int value)
    {
        
        if (slot == AddressQ)
        {
            AddressQ = null;
            AddressV = value;
        }
        if (Op.executionType != ExecutionTypes.Vector)
        {
            if (slot == Q)
            {
                Q = null;
                V = value;
            }
            if (AddressQ == null && Q == null)
                ready = true;
        }
        else
        {
            if(Busy && Op != null && AddressQ == null)
                ready = true;
            if (Qs != null)
            {
                for (int i = 0; i < Qs.Count; i++)
                {
                    if (Qs[i] != null)
                    {
                        ready = false;
                    }
                }
            }
        }
    }

    public void CDBupdate(int slot, int[] values)
    {
        for (int i = 0; i < Qs.Count; i++)
        {
            if (slot == Qs[i])
            {
                Qs[i] = null;
                for (int j = 0; j < 4; j++)
                {
                    Vs[i*4+j] = values[j];
                }
            }
        }
        if(Busy && Op != null && AddressQ == null)
            ready = true;
        for (int i = 0; i < Qs.Count; i++)
        {
            if (Qs[i] != null)
            {
                ready = false;
            }
        }
    }

    public (Instruction, List<int>) getInstructionForExecution()
    {
        if (Op.executionType != ExecutionTypes.Vector)
        {
            List<int> returnV = new List<int>();
            if (AddressV != null)
                returnV.Add((int) AddressV);
            else
                returnV.Add(-1);
            if (V != null)
                returnV.Add((int) V);
            else
                returnV.Add(-1);
            return (Op, returnV);
        }
        else
        {
            List<int> returnV = new List<int>();
            if (Op.GetType() == typeof(VStoreR) || Op.GetType() == typeof(VLoadR))
            {
                returnV.Add((int)AddressV);
            }
            if (Vs != null)
            {
                foreach (int? item in Vs)
                {
                    if(item != null)
                        returnV.Add((int) item);
                }
            }
            return (Op, returnV);
        }
    }
}