namespace ProcessorSim.HardwareResources;

public class FetchUnit
{
    public (int, bool) fetch(Resources resources, bool verbose=true)
    {
        try
        {
            //bool emptyRegisterFound = false;
            int registerIndex = 31;
            /*
            while (!emptyRegisterFound)
            {
                if (resources.registers[registerIndex].available)
                    emptyRegisterFound = true;
                else
                    registerIndex++;
            }
            */
            if (verbose)
            {
                Console.WriteLine("Fetch Debug:");
                Console.WriteLine("  PC Value: {0}", resources.pc.getValue());
            }
            string instruction = resources.instructionMemory[resources.pc.getValue()].getInstruction();
            resources.registers[registerIndex]
                .setInstruction(instruction);
            resources.registers[registerIndex].available = false;
            resources.pc.setValue(resources.pc.getValue() + 1);
            string instructionType = instruction.Split(" ")[0];
            string[] stringMatches = new[] {"Load", "Compare", "Copy", "Add", "Subtract", "Divide", "Multiply", "Not"};
            bool newArchitecturalRegisterNeeded = stringMatches.Any(s=>instructionType.Contains(s));
            return (registerIndex, newArchitecturalRegisterNeeded);
        }
        catch (Exception e)
        {
            return (-1, false);
        }
    }
}