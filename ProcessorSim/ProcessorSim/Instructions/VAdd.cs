using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class VAdd : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public VRegister targetRegister { get; set; }
    public int[] result { get; set; }
    public int registerFile { get; set; }
    public List<VRegister> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    
    public VAdd(VRegister target, VRegister register1, VRegister register2)
    {
        inputRegisters = new List<VRegister>();
        inputRegisters.Add(register1);
        inputRegisters.Add(register2);
        this.targetRegister = target;
        this.executionType = ExecutionTypes.SimpleArithmetic;
    }
    public bool execute(Resources resources, List<int[]> args)
    {
        int[] vals1 = args[0];
        int[] vals2 = args[1];
        result = new int[4];
        for (int i = 0; i < 4; i++)
        {
            this.result[i] = vals1[i] + vals2[i];
        }
        return true;
    }
}