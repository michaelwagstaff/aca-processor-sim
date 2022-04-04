using ProcessorSim.Enums;
using ProcessorSim.Instructions;

namespace ProcessorSim.HardwareResources;

public class DecodeUnit
{
    private int cyclesToCompletion;

    public DecodeUnit()
    {
    }
    public (Instruction, Dictionary<Register, int>) decode(Resources resources, int? instructionRegister, bool newRegisterNeeded)
    {
        if (instructionRegister == null || instructionRegister == -1)
            return (null, null);
        string rawInstruction = resources.registerFile.getPhysicalRegisters()[(int) instructionRegister].getInstruction();
        resources.registerFile.getPhysicalRegisters()[(int) instructionRegister].available = true;
        if (rawInstruction == null)
        {
            return (new Blank(), new Dictionary<Register, int>());
        }
        string opCode = rawInstruction.Split(" ")[0];
        string op1 = null;
        string op2 = null;
        string op3 = null;
        Register reg1 = null;
        Register reg2 = null;
        Register reg3 = null;
        Register oldReg1 = null;
        Dictionary<Register, int> unclearedDependencies = new Dictionary<Register, int>();
        try
        {
            op1 = rawInstruction.Split(" ")[1];
            try
            {
                if (op1[0] == 'r')
                {
                    reg1 = resources.registers[Int32.Parse(op1.Substring(1))];
                    oldReg1 = resources.registerFile.getPhysicalRegister(resources.registerFile.getCurrentFile(), reg1);
                    if(newRegisterNeeded)
                        resources.registerFile.addFile(reg1);
                    reg1 = resources.registerFile.getPhysicalRegister(resources.registerFile.getCurrentFile(), reg1);
                }
            }
            catch { }
        }
        catch { }
        try
        {
            op2 = rawInstruction.Split(" ")[2];
            try
            {
                if (op2[0] == 'r')
                {
                    reg2 = resources.registers[Int32.Parse(op2.Substring(1))];
                    reg2 = resources.registerFile.getPhysicalRegister(resources.registerFile.getCurrentFile(), reg2);
                }
            }
            catch { }
        }
        catch { }

        try
        {
            op3 = rawInstruction.Split(" ")[3];
            try
            {
                if (op3[0] == 'r')
                {
                    reg3 = resources.registers[Int32.Parse(op3.Substring(1))];
                    reg3 = resources.registerFile.getPhysicalRegister(resources.registerFile.getCurrentFile(), reg3);
                }
            }
            catch
            {
            }
        }
        catch
        { }
        Instruction instruction = findInstruction(opCode, op1, op2, op3, reg1, reg2, reg3, oldReg1);
        instruction.registerFile = resources.registerFile.getCurrentFile();
        if (resources.verbose)
        {
            Console.WriteLine("  {0}", rawInstruction);
        }
        // Add to the uncleared dependencies if *we* depend on the register
        // Add to the instructions in flight if *we* may be the dependency
        try
        {
            if (!newRegisterNeeded)
            {
                unclearedDependencies[reg1] = resources.registerInstructionsInFlight[reg1];
            }
            //Console.WriteLine("Uncleared Dependencies: {0} count {1}", reg1.index, resources.registerInstructionsInFlight[reg1]);
            if (newRegisterNeeded)
            {
                resources.registerInstructionsInFlight[reg1]++;
                //Console.WriteLine("Instruction {0} blocking register {1}", instruction, reg1.index);
            }
        } catch {}
        try
        {
            if(oldReg1 != reg1)
                unclearedDependencies[oldReg1] = resources.registerInstructionsInFlight[oldReg1];
        } catch {}
        try
        {
            unclearedDependencies[reg2] = resources.registerInstructionsInFlight[reg2];
        } catch {}
        try
        {
            unclearedDependencies[reg3] = resources.registerInstructionsInFlight[reg3];
        } catch {}

        return (instruction, unclearedDependencies);
    }

    private Instruction findInstruction(string opCode, string op1, string op2, string op3, Register reg1, Register reg2, Register reg3, Register oldReg1)
    {
        switch(opCode)
        {
            case "Add":
                return new Add(reg1, oldReg1, reg2);
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
                return new Divide(reg1, oldReg1, reg2);
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
                return new Multiply(reg1, oldReg1, reg2);
            case "Not":
                return new Not(reg1, oldReg1);
            case "Print":
                return new Print(reg1);
            case "Store":
                return new Store(reg1, Int32.Parse(op2));
            case "StoreR":
                return new StoreR(reg1, reg2);
            case "Subtract":
                return new Subtract(reg1, oldReg1, reg2);
        }
        return new Blank();
    }
}