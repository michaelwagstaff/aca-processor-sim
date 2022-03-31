using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class DecodeUnit
{
    private int cyclesToCompletion;

    public DecodeUnit()
    {
    }
    public (Instruction, List<Register>) decode(Resources resources, int? instructionRegister)
    {
        if (instructionRegister == null)
            return (null, null);
        string rawInstruction = resources.registers[(int) instructionRegister].getInstruction();
        resources.registers[(int) instructionRegister].available = true;
        if (rawInstruction == null)
        {
            return (new Blank(), new List<Register>());
        }
        string opCode = rawInstruction.Split(" ")[0];
        string op1 = null;
        string op2 = null;
        string op3 = null;
        Register reg1 = null;
        Register reg2 = null;
        Register reg3 = null;
        List<Register> unclearedDependencies = new List<Register>();
        try
        {
            op1 = rawInstruction.Split(" ")[1];
            try
            {
                reg1 = resources.registers[Int32.Parse(op1.Substring(1))];
                if(resources.dataHazards[reg1])
                    unclearedDependencies.Add(reg1);
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
                if(resources.dataHazards[reg2])
                    unclearedDependencies.Add(reg2);
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
            catch
            {
            }
        }
        catch
        { }
        Instruction instruction = findInstruction(opCode, op1, op2, op3, reg1, reg2, reg3);
        if (instruction.GetType() == typeof(Load) || instruction.GetType() == typeof(IndexedLoad) || 
            instruction.GetType() == typeof(LoadI) || instruction.GetType() == typeof(LoadR))
        {
            resources.registerFile.addFile(reg1);
        }
        instruction.registerFile = resources.registerFile.getCurrentFile();
        try
        {
            if (resources.dataHazards[resources.registerFile.getPhysicalRegister(instruction.registerFile, reg1)])
                unclearedDependencies.Add(reg1);
        } catch {}

        try
        {
            if (resources.dataHazards[resources.registerFile.getPhysicalRegister(instruction.registerFile, reg2)])
                unclearedDependencies.Add(reg2);
        } catch {}

        try
        {
            if (resources.dataHazards[resources.registerFile.getPhysicalRegister(instruction.registerFile, reg3)])
                unclearedDependencies.Add(reg3);
        } catch {}

        return (instruction, unclearedDependencies);
    }

    private Instruction findInstruction(string opCode, string op1, string op2, string op3, Register reg1, Register reg2, Register reg3)
    {
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
            case "IndexedLoad":
                return new IndexedLoad(reg1, reg2, reg3);
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