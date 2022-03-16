using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class DecodeUnit
{
    private int cyclesToCompletion;

    public DecodeUnit()
    {
    }
    public Instruction decode(Resources resources, int? instructionRegister)
    {
        if (instructionRegister == null)
            return null;
        string rawInstruction = resources.registers[(int) instructionRegister].getInstruction();
        resources.registers[(int) instructionRegister].available = true;
        if (rawInstruction == null)
        {
            return new Blank();
        }
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
            case "End":
                return new End();
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
}