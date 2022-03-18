namespace ProcessorSim.HardwareResources;

public class RegisterFile
{
    private Dictionary<int, Dictionary<int, (Register, int)>> internalFile;
    // Int represents which mapping we're using to allow r1 to be mapped multiple times for instructions executing simultaneously
    // This int then provides the real current dictionary, types yet to be firmly decided, key is register val user included in code
    // The result of this dictionary mapping is a tuple, saying what physical register we use as well as what data dependency we have.
    // This may be updated with more complex structs, but I very much hope to avoid this.
    private Dictionary<Register, bool> availableRegisters;
    private Resources resources;
    private int count = 0;
    
    public RegisterFile(Resources resources)
    {
        this.internalFile = new Dictionary<int, Dictionary<int, (Register, int)>>();
        this.resources = resources;
    }

    public int addFile(int logicalRegister)
    {
        this.internalFile[count] = new Dictionary<int, (Register, int)>();
        Register newReg = this.resources.registers[logicalRegister]; // Temporarily don't do any re-assigning
        this.internalFile[count][logicalRegister] = (newReg, -1); // Use -1 if we use an immediate load
        count++;
        return count - 1;
    }

    public Register getPhysicalRegister(int fileNum, int logicalRegister)
    {
        return this.internalFile[fileNum][logicalRegister].Item1;
    }
}