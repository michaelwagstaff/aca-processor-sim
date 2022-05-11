namespace ProcessorSim.HardwareResources;

public class FetchUnit
{
    public (int, (bool, (bool, int))) fetch(Resources resources, int superscalarCount)
    {
        try
        {
            int regAddress = 0;
            for (int i = 0; i < superscalarCount; i++)
            {
                if (resources.registers[31 - i].available)
                {
                    regAddress = 31 - i;
                    break;
                }
            }
            if (resources.verbose)
            {
                Console.WriteLine("Fetch Debug:");
                Console.WriteLine("  PC Value: {0}", resources.pc.getValue());
            }
            string instruction = resources.instructionMemory[resources.pc.getValue()].getInstruction();
            Console.WriteLine(instruction);
            if (instruction.Contains("MonitorStart"))
            {
                resources.monitor.start();
                instruction = "";
            }
            while (instruction == "")
            {
                resources.pc.setValue(resources.pc.getValue() + 1);
                instruction = resources.instructionMemory[resources.pc.getValue()].getInstruction();
            }
            resources.registers[regAddress].setInstruction(instruction);
            resources.registers[regAddress].available = false;
            bool predictedTaken = false;
            int continuationAddress = -1;
            if (instruction.Contains("CondBranch"))
            {
                int possibleBranchAddress = Int32.Parse(instruction.Split(" ")[2]) - 1;
                continuationAddress = resources.pc.getValue() + 1;
                if (possibleBranchAddress < resources.pc.getValue())
                {
                    resources.pc.setValue(possibleBranchAddress);
                    predictedTaken = true;
                }
                else
                {
                    resources.pc.setValue(resources.pc.getValue() + 1);
                }
            }
            else if (instruction.Contains("Branch"))
            {
                resources.pc.setValue(Int32.Parse(instruction.Split(" ")[1]) - 1);
            }
            else
            {
                resources.pc.setValue(resources.pc.getValue() + 1);
            }

            string instructionType = instruction.Split(" ")[0];
            string[] stringMatches = new[] {"Load", "Compare", "Copy", "Add", "Subtract", "Divide", "Multiply", "Not"};
            bool newArchitecturalRegisterNeeded = stringMatches.Any(s=>instructionType.Contains(s));
            //bool possibleBranch = instructionType.Contains("Branch");
            bool possibleBranch = false;
            return (regAddress, (newArchitecturalRegisterNeeded, (predictedTaken, continuationAddress)));
        }
        catch (Exception e)
        {
            return (-1, (false, (false, -1)));
        }
    }
}