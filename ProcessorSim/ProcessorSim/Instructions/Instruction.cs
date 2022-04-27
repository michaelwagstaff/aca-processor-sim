using ProcessorSim.Enums;
using ProcessorSim.HardwareResources;
namespace ProcessorSim.Instructions;

public interface Instruction
{
    ExecutionTypes executionType { get; set; }
    Register targetRegister { get; set; }
    int result { get; set; }
    int registerFile { get; set; }
    List<Register> inputRegisters { get; set; }
    (ExecutionTypes, int) reservationStation { get; set; }
    public bool execute(Resources resources, List<int> vals)
    {
        return true;
    }

    public int getVal(Resources resources, Register register)
    {
        int val = register.getValue();
        if (register != null && resources.forwardedResults[register] != null)
        {
            val = (int) resources.forwardedResults[register];
            resources.forwardedResults[register] = null;
        }

        return val;
    }
}