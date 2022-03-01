using System.Runtime.CompilerServices;
using ProcessorSim.Instructions;
using ProcessorSim.HardwareResources;
using System.Threading;

namespace ProcessorSim;

class ProcessorSim
{
    public static void Main(string[] args)
    {
        Resources resources = new Resources(32, 512, 1024);
        resources.setExecutionUnits(1);
        loadProgram(resources);
        try
        {
            while (true)
            {
                tick(resources);
                Thread.Sleep(10);
            }
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Computation Finished!");
        }
    }

    public static void loadProgram(Resources resources)
    {
        // StreamReader reader = new StreamReader(@"Programs/bubblesort.mpl");
        // StreamReader reader = new StreamReader(@"Programs/fact.mpl");
        StreamReader reader = new StreamReader(@"Programs/gcd-original.mpl");
        // StreamReader reader = new StreamReader(@"Programs/vectoradd.mpl");
        int i = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            resources.instructionMemory[i].setInstruction(line);
            i++;
        }
        resources.pc.setValue(0);
    }
    public static void tick(Resources resources)
    {
        int instructionRegister = fetch(resources);
        Instruction decodedInstruction = decode(resources, instructionRegister);
        execute(resources, decodedInstruction);
    }

    public static int fetch(Resources resources)
    {
        bool emptyRegisterFound = false;
        int registerIndex = 16;
        while (!emptyRegisterFound)
        {
            if (resources.registers[registerIndex].available)
                emptyRegisterFound = true;
            else
                registerIndex++;
        }
        resources.registers[registerIndex].setInstruction(resources.instructionMemory[resources.pc.getValue()].getInstruction());
        resources.pc.setValue(resources.pc.getValue() + 1);
        return registerIndex;
    }

    public static Instruction decode(Resources resources, int instructionRegister)
    {
        string rawInstruction = resources.registers[instructionRegister].getInstruction();
        // Console.WriteLine(resources.pc.getValue());
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
                return new Add(reg1, reg2);
            case "Blank":
                return new Blank();
            case "Branch":
                return new Branch(reg1);
            case "CondBranch":
                return new CondBranch(reg1, reg2);
            case "Compare":
                return new Compare(reg1, reg2, reg3);
            case "CompareI":
                return new CompareI(reg1, reg2, Int32.Parse(op3));
            case "CompareLT":
                return new CompareLT(reg1, reg2, reg3);
            case "Copy":
                return new Copy(reg1, reg2);
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
            case "Not":
                return new Not(reg1);
            case "Print":
                return new Print(reg1);
            case "Store":
                return new Store(reg1, Int32.Parse(op2));
            case "StoreR":
                return new StoreR(reg1, reg2);
            case "Subtract":
                return new Subtract(reg1, reg2);
        }
        return new Blank();
    }

    public static void execute(Resources resources, Instruction instruction)
    {
        // Console.WriteLine(instruction.ToString());
        resources.executionUnits[0].execute(resources, instruction);
    }

    public static void memory(Resources resources, Instruction instruction)
    {
        
    }
    public static void writeback(Resources resources, Instruction instruction)
    {
        
    }
}