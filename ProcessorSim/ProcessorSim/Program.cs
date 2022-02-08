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
        Instruction decodedInstruction = decode(resources, instructionRegister);
        execute(resources, decodedInstruction);
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
        string rawInstruction = resources.registers[instructionRegister].getInstruction();
        string opCode = rawInstruction.Split(" ")[0];
        string op1 = rawInstruction.Split(" ")[1];
        string op2 = rawInstruction.Split(" ")[2];
        switch(opCode)
        {
            case "Add":
                return new Add(resources.registers[Int32.Parse(op1)], resources.registers[Int32.Parse(op2)]);
        }
        return new Add(null, null);
    }

    public static void execute(Resources resources, Instruction instruction)
    {
        instruction.execute(resources);
    }
}