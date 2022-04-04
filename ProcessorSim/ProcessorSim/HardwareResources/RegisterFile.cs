using System.Security.AccessControl;

namespace ProcessorSim.HardwareResources;

public class RegisterFile
{
    private Dictionary<int, Dictionary<Register, (Register, int)>> internalFile;
    // Int represents which mapping we're using to allow r1 to be mapped multiple times for instructions executing simultaneously
    // This int then provides the real current dictionary, types yet to be firmly decided, key is register val user included in code
    // The result of this dictionary mapping is a tuple, saying what physical register we use as well as what data dependency we have.
    // This may be updated with more complex structs, but I very much hope to avoid this.
    private Register[] physicalRegisters;
    private Queue<Register> availableRegisters;
    private Resources resources;
    private int count = 0;
    
    public RegisterFile(Resources resources)
    {
        this.internalFile = new Dictionary<int, Dictionary<Register, (Register, int)>>();
        this.physicalRegisters = new Register[64];
        for (int i = 0; i < physicalRegisters.Length; i++)
        {
            this.physicalRegisters[i] = new Register();
            this.physicalRegisters[i].index = i;
        }
        this.availableRegisters = new Queue<Register>();
        for (int i = 1 + resources.decodeUnits.Count; i < physicalRegisters.Length; i++)
        {
            // Starting at 1 + superscalar count to omit pc and in flight instruction
            this.availableRegisters.Enqueue(physicalRegisters[i]);
        }
        this.resources = resources;
    }

    public int addFile(Register logicalRegister)
    {
        this.internalFile[count] = new Dictionary<Register, (Register, int)>();
        if (count != 0)
        {
            foreach (KeyValuePair<Register, (Register, int)> registerMapping in this.internalFile[count - 1])
            {
                this.internalFile[count][registerMapping.Key] = registerMapping.Value;
            }
        }

        Register newReg = this.availableRegisters.Dequeue(); // Temporarily don't do any re-assigning
        if(this.internalFile[count].ContainsKey(logicalRegister))
            this.availableRegisters.Enqueue(this.internalFile[count][logicalRegister].Item1);
        this.internalFile[count][logicalRegister] = (newReg, -1); // Use -1 if we use an immediate load
        count++;
        return count - 1;
    }

    public Dictionary<Register, (Register, int)> getFile(int index)
    {
        try
        {
            return this.internalFile[index];
        }
        catch (Exception e)
        {
            return null;
        } 
    }

    public Register getPhysicalRegister(int fileNum, Register logicalRegister)
    {
        try
        {
            return this.internalFile[fileNum][logicalRegister].Item1;
        }
        catch
        {
            return null;
        }
    }

    public int getCurrentFile()
    {
        return count - 1;
    }

    public void printMapping()
    {
        Console.WriteLine("  Current Register Mappings");
        foreach(KeyValuePair<Register, (Register, int)> registerMapping in this.internalFile[getCurrentFile()])
        {
            Console.WriteLine("    {0} |-> {1}, val {2}", registerMapping.Key.index, registerMapping.Value.Item1.index, registerMapping.Value.Item1.getValue());
        }
    }

    public Register[] getPhysicalRegisters()
    {
        return this.physicalRegisters;
    }
}