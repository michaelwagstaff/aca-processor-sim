using ProcessorSim.Instructions;
using ProcessorSim.HardwareResources;

namespace ProcessorSim;

class ProcessorSim
{
    public static void Main(string[] args)
    {
        Resources resources = new Resources(32, 512, 1024);
        
    }

    public static void tick(Resources resources)
    {
        int instructionRegister = fetch(resources);
        decode(resources, instructionRegister);
        execute(resources);
    }

    public static int fetch(Resources resources)
    {
        resources.pc.setValue(resources.pc.getValue() + 1);
        bool emptyRegisterFound = false;
        int registerIndex = 0;
        while (!emptyRegisterFound)
        {
            if (resources.registers[registerIndex].available)
                emptyRegisterFound = true;
            else
                registerIndex++;
        }
        resources.registers[registerIndex].setValue(resources.instructionMemory[resources.pc.getValue()].getValue());
        return registerIndex;
    }

    public static Instruction decode(Resources resources, int instructionRegister)
    {
        int rawInstruction = resources.registers[instructionRegister].getValue();
        // Actually decode
        return new Add();
    }

    public static void execute(Resources resources, Instruction instruction)
    {
        instruction.execute(resources);
    }
}