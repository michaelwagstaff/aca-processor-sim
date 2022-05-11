namespace ProcessorSim.HardwareResources;

public class Monitor
{
    private int instructionsExecuted;
    private int cyclesTaken;
    private int vectorInstructionsExecuted;

    public void incrementInstructionsExecuted()
    {
        instructionsExecuted++;
    }
    public void incrementCyclesTaken()
    {
        cyclesTaken++;
    }

    public void incrementVectorInstructions()
    {
        vectorInstructionsExecuted++;
    }

    public int getInstructionsExecuted()
    {
        return instructionsExecuted;
    }
    public int getCyclesTaken()
    {
        return cyclesTaken;
    }

    public int getVectorInstructionsExecuted()
    {
        return vectorInstructionsExecuted;
    }

    public bool vectorInstructionsUsed()
    {
        return vectorInstructionsExecuted != 0;
    }

    public void reset()
    {
        // Enables benchmarking of core kernels
        instructionsExecuted = 0;
        cyclesTaken = 0;
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