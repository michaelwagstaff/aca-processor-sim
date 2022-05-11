using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class VPrint : VInstruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    public int[] vectorResult { get; set; }

    public VPrint(Register register)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register);
        this.executionType = ExecutionTypes.Vector;
    }

    public bool execute(Resources resources, List<int> args)
    {
        /*
        if (reg.isInstruction)
        {
            Console.WriteLine(reg.getInstruction());
        }
        else
        {*/
        for (int i = 0; i < args.Count; i++)
        {
            if(i != 0)
                Console.Write(",");
            Console.Write(args[i]);
        }
        Console.WriteLine();

        vectorResult = args.ToArray();
        //}

        return true;
    }
}