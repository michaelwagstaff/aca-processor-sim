using ProcessorSim.Enums;

namespace ProcessorSim.Instructions;

public class End : Instruction
{
    public ExecutionTypes executionType { get; set; }
    public End()
    {
        this.executionType = ExecutionTypes.General;
    }

    public bool execute(Resources resources)
    {
        Console.WriteLine();
        Console.WriteLine("-- Fin --");
        Console.WriteLine();
        Environment.Exit(0);
        return true;
    }
}