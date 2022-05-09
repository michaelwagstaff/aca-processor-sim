namespace ProcessorSim.HardwareResources;

public class Monitor
{
    private int instructionsExecuted;
    private int cyclesTaken;

    public void incrementInsructionsExecuted()
    {
        instructionsExecuted++;
    }
    public void incrementCyclesTaken()
    {
        cyclesTaken++;
    }

    public int getInstructionsExecuted()
    {
        return instructionsExecuted;
    }
    public int getCyclesTaken()
    {
        return cyclesTaken;
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
}