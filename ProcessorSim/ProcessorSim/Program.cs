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
        int rawInstruction = resources.registers[instructionRegister].getValue();
        rawInstruction = rawInstruction >> 16;
        // First 6 bits are opcode
        // Next 5 for operand 1
        // Final 5 for operand 2
        int opCode = rawInstruction >> 10;
        int operand1 = (rawInstruction >> 5) & 31;
        int operand2 = rawInstruction & 31;
        if ((opCode >> 5) == 1)
        {
            // Arithmetic
            if ((opCode >> 3 & 3) == 0)
            {
                // Add
            }
            if ((opCode >> 3 & 3) == 1)
            {
                // Subtract
            }
            if ((opCode >> 3 & 3) == 2)
            {
                // Multiply
            }
            if ((opCode >> 3 & 3) == 3)
            {
                // Divide
            }
        }
        else
        {
            if ((opCode >> 3 & 3) == 0)
            {
                // Load
            }

            if ((opCode >> 3 & 3) == 1)
            {
                // Store
            }
            // Everything Else
        }
        // Actually decode
        return new Add();
    }

    public static void execute(Resources resources, Instruction instruction)
    {
        instruction.execute(resources);
    }
}