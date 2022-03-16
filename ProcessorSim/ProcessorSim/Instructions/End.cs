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
        Console.WriteLine("Program Stats:");
        Console.WriteLine("Instructions Executed: {0}", resources.monitor.getInstructionsExecuted());
        Console.WriteLine("Cycles Executed: {0}", resources.monitor.getCyclesTaken());
        Console.WriteLine("IPC: {0}", resources.monitor.getIPC());
        
        Environment.Exit(0);
        return true;
    }
}