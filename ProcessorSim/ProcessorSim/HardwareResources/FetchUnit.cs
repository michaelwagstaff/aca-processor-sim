namespace ProcessorSim.HardwareResources;

public class FetchUnit
{
    public (int, (bool, bool)) fetch(Resources resources, int superscalarCount)
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
            resources.registers[regAddress].setInstruction(instruction);
            resources.registers[regAddress].available = false;
            resources.pc.setValue(resources.pc.getValue() + 1);
            string instructionType = instruction.Split(" ")[0];
            string[] stringMatches = new[] {"Load", "Compare", "Copy", "Add", "Subtract", "Divide", "Multiply", "Not"};
            bool newArchitecturalRegisterNeeded = stringMatches.Any(s=>instructionType.Contains(s));
            bool possibleBranch = instructionType.Contains("Branch");
            return (regAddress, (newArchitecturalRegisterNeeded, possibleBranch));
        }
        catch (Exception e)
        {
            return (-1, (false, false));
        }
    }
}