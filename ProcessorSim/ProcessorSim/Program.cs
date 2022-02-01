
using ProcessorSim.Instructions;

namespace ProcessorSim;

class ProcessorSim
{
    public static void Main(string[] args)
    {
        Resources resources = new Resources(32, 512, 1024);
        
    }

    public static void tick(Resources resources)
    {
        
    }

    public int fetch(Resources resources)
    {
        resources.pc.setValue(resources.pc.getValue() + 1);
        return resources.pc.getValue();
    }

    public static Instruction decode()
    {
        return new Add();
    }

    public static void execute(Resources resources, Instruction instruction)
    {
        instruction.execute(resources);
    }
}