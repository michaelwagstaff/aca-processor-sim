namespace ProcessorSim.HardwareResources;

public class Monitor
{
    private int instructionsExecuted;
    private int cyclesTaken;
    private int vectorInstructionsExecuted;
    private bool enabled;

    public Monitor()
    {
        enabled = false;
    }

    public void incrementInstructionsExecuted()
    {
        if(enabled)
            instructionsExecuted++;
    }
    public void incrementCyclesTaken()
    {
        if(enabled)
            cyclesTaken++;
    }

    public void incrementVectorInstructions()
    {
        if(enabled)
            vectorInstructionsExecuted++;
    }
    public bool vectorInstructionsUsed()
    {
        return vectorInstructionsExecuted != 0;
    }

    public void start()
    {
        // Enables benchmarking of core kernels
        enabled = true;
    }

    public float getIPC()
    {
        return (float) instructionsExecuted / (float) cyclesTaken;
    }
    public float getAdjustedIPC()
    {
        return (float) (instructionsExecuted + 3 * vectorInstructionsExecuted) / (float) cyclesTaken;
    }

    public void printStats()
    {
        Console.WriteLine("Instructions Executed: {0}", instructionsExecuted);
        Console.WriteLine("Cycles Executed: {0}", cyclesTaken);
        Console.WriteLine("IPC: {0}", getIPC());
        if (vectorInstructionsUsed())
        {
            Console.WriteLine();
            Console.WriteLine("Adjusted for Vector Ops");
            Console.WriteLine("Instructions Executed: {0}", instructionsExecuted + 3 * vectorInstructionsExecuted);
            Console.WriteLine("Cycles Executed: {0}", cyclesTaken);
            Console.WriteLine("IPC: {0}", getAdjustedIPC());
        }
    }
}