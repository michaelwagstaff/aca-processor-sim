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

    public float getIPC()
    {
        return (float) instructionsExecuted / (float) cyclesTaken;
    }
}