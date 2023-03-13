using System.Security.AccessControl;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class RegisterFile
{
    private Dictionary<Register, (ExecutionTypes, int)?> internalFile;
    private Resources resources;
    private int count = 0;
    
    public RegisterFile(Resources resources)
    {
        
    }

    public void printMapping()
    {
        Console.WriteLine("  Current Registers Waiting Results");
        foreach(KeyValuePair<Register, (ExecutionTypes, int)?> registerMapping in this.internalFile)
        {
            Console.WriteLine("    {0} Waiting On: {1}", registerMapping.Key.index, registerMapping.Value.ToString());
        }
    }
}