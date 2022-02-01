using ProcessorSim.HardwareResources;
namespace ProcessorSim;

public class Resources
{
    public Register[] registers;
    public Register pc;

    public MemorySlot[] instructionMemory;
    public MemorySlot[] dataMemory;
    public Resources(int regCount, int instCount, int dataCount)
    {
        registers = new Register[regCount];
        for(int i = 0; i < registers.Length; i++)
        {
            registers[i] = new Register();
        }
        pc = registers[0];
        
        instructionMemory = new MemorySlot[instCount];
        for(int i = 0; i < instructionMemory.Length; i++)
        {
            instructionMemory[i] = new MemorySlot();
        }
        
        dataMemory = new MemorySlot[dataCount];
        for(int i = 0; i < dataMemory.Length; i++)
        {
            dataMemory[i] = new MemorySlot();
        }
    }
}