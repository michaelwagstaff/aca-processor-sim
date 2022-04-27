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
        this.internalFile = new Dictionary<Register, (ExecutionTypes, int)?>();
        this.resources = resources;
    }

    public (ExecutionTypes, int)? getDependantStation(Register register)
    {
        if(register != null)
            return internalFile[register];
        return null;
    }

    public void setDependantStation(Register dest, (ExecutionTypes, int) station)
    {
        if (dest != null)
            this.internalFile[dest] = station;
    }

    public void printMapping()
    {
        Console.WriteLine("  Current Registers Waiting Results");
        foreach(KeyValuePair<Register, (ExecutionTypes, int)?> registerMapping in this.internalFile)
        {
            Console.WriteLine("    {0} Waiting On: {1}", registerMapping.Key.index, registerMapping.Value.ToString());
        }
    }

    public void CDBUpdate((ExecutionTypes, int) station, int value)
    {
        foreach (KeyValuePair<Register, (ExecutionTypes, int)?> fileEntry in internalFile)
        {
            if (station == fileEntry.Value)
            {
                fileEntry.Key.setValue(value);
                internalFile[fileEntry.Key] = null;
            }
        }
    }
}