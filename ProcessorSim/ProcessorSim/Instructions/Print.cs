using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;

namespace ProcessorSim.Instructions;

public class Print : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public Register targetRegister { get; set; }
    public int result { get; set; }
    public int registerFile { get; set; }
    public List<Register> inputRegisters { get; set; }
    public int reorderBuffer { get; set; }
    private Register reg;

    public Print(Register register)
    {
        inputRegisters = new List<Register>();
        inputRegisters.Add(register);
        this.reg = register;
        this.executionType = ExecutionTypes.General;
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
            Console.WriteLine(args[i]);
        }
        //}

        return true;
    }
}