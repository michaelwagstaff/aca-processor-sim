namespace ProcessorSim.HardwareResources;

public class RegisterFile
{
    private Dictionary<int, Dictionary<Register, (Register, int)>> internalFile;
    // Int represents which mapping we're using to allow r1 to be mapped multiple times for instructions executing simultaneously
    // This int then provides the real current dictionary, types yet to be firmly decided, key is register val user included in code
    // The result of this dictionary mapping is a tuple, saying what physical register we use as well as what data dependency we have.
    // This may be updated with more complex structs, but I very much hope to avoid this.
    private Dictionary<Register, bool> availableRegisters;
    private Resources resources;
    private int count = 0;
    
    public RegisterFile(Resources resources)
    {
        this.internalFile = new Dictionary<int, Dictionary<Register, (Register, int)>>();
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

        Register newReg = logicalRegister; // Temporarily don't do any re-assigning
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
}