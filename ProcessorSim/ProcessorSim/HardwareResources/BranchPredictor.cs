namespace ProcessorSim.HardwareResources;

public class BranchPredictor
{
    private Dictionary<int, int> previousBranches;
    public BranchPredictor()
    {
        previousBranches = new Dictionary<int, int>();
    }

    public void noteResult(int pc, int address)
    {
        previousBranches[pc] = address;
    }

    public int? getResults(int pc)
    {
        /*if (previousBranches.ContainsKey(pc))
            return previousBranches[pc];*/
        return null;
    }
}