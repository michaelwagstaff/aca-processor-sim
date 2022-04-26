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
    public (ExecutionTypes, int) reservationStation { get; set; }
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
        if (reg.isInstruction)
        {
            Console.WriteLine(reg.getInstruction());
        }
        else
        {
            Instruction instruction = (Instruction) this;
            int val = args[0];
            Console.WriteLine(val);
        }

        return true;
    }
}