namespace ProcessorSim.HardwareResources;

public class FetchUnit
{
    public (int, (bool, bool)) fetch(Resources resources, int count)
    {
        try
        {
            if (resources.verbose)
            {
                Console.WriteLine("Fetch Debug:");
                Console.WriteLine("  PC Value: {0}", resources.pc.getValue());
            }
            string instruction = resources.instructionMemory[resources.pc.getValue()].getInstruction();
            resources.registerFile.getPhysicalRegisters()[count]
                .setInstruction(instruction);
            resources.registerFile.getPhysicalRegisters()[count].available = false;
            resources.pc.setValue(resources.pc.getValue() + 1);
            string instructionType = instruction.Split(" ")[0];
            string[] stringMatches = new[] {"Load", "Compare", "Copy", "Add", "Subtract", "Divide", "Multiply", "Not"};
            bool newArchitecturalRegisterNeeded = stringMatches.Any(s=>instructionType.Contains(s));
            bool possibleBranch = instructionType.Contains("Branch");
            return (count, (newArchitecturalRegisterNeeded, possibleBranch));
        }
        catch (Exception e)
        {
            return (-1, (false, false));
        }
    }
}