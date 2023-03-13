using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class VAdd : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int[] vectorResult { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    
    public VAdd(Register target, Register register1, Register register2)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register1);
        inputRegisters.Add(register2);
        this.targetRegister = target;
        this.executionType = ExecutionTypes.Vector;
    }
    public bool execute(Resources resources, List<int> args)
    {
        vectorResult = new int[4];
        for (int i = 0; i < 4; i++)
        {
            vectorResult[i] = args[i] + args[i+4];
        }
        return true;
    }
}