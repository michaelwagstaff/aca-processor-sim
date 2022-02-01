namespace ProcessorSim;

class ProcessorSim
{
    public static void Main(string[] args)
    {
        Register[] registers = new Register[32];
        for(int i = 0; i < registers.Length; i++)
        {
            registers[i] = new Register();
        }
    }
}