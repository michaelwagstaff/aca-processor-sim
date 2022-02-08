using System.Runtime.CompilerServices;
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
        string op1 = null;
        string op2 = null;
        string op3 = null;
        Register reg1 = null;
        Register reg2 = null;
        Register reg3 = null;
        try
        {
            op1 = rawInstruction.Split(" ")[1];
            try
            {
                reg1 = resources.registers[Int32.Parse(op1.Substring(1))];
            }
            catch { }
        }
        catch { }
        try
        {
            op2 = rawInstruction.Split(" ")[2];
            try
            {
                reg2 = resources.registers[Int32.Parse(op2.Substring(1))];
            }
            catch { }
        }
        catch { }
        try
        {
            op3 = rawInstruction.Split(" ")[3];
            try
            {
                reg3 = resources.registers[Int32.Parse(op3.Substring(1))];
            }
            catch { }
        }
        catch { }
        
        switch(opCode)
        {
            case "Add":
                return new Add(resources.registers[Int32.Parse(op1)], resources.registers[Int32.Parse(op2)]);
            case "Branch":
                return new Branch(resources, reg1);
            case "CondBranch":
                return new CondBranch(resources, reg1, reg2);
            case "Compare":
                return new Compare(reg1, reg2, reg3);
            case "CompareI":
                return new CompareI(reg1, reg2, Int32.Parse(op3));
            case "Divide":
                return new Divide(reg1, reg2);
            case "Load":
                return new Load(reg1, Int32.Parse(op2));
            case "LoadI":
                return new LoadI(reg1, Int32.Parse(op2));
            case "LoadR":
                return new LoadR(reg1, reg2);
            case "MarkAvailable":
                return new MarkAvailable(reg1);
            case "Multiply":
                return new Multiply(reg1, reg2);
            case "Store":
                return new Store(reg1, Int32.Parse(op2));
            case "StoreR":
                return new StoreR(reg1, reg2);
            case "Subtract":
                return new Subtract(reg1, reg2);
        }
        return new Add(null, null);
    }

    public static void execute(Resources resources, Instruction instruction)
    {
        instruction.execute(resources);
    }
}