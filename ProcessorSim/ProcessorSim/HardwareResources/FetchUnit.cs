namespace ProcessorSim.HardwareResources;

public class FetchUnit
{
    public int fetch(Resources resources, bool verbose=false)
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
            Console.WriteLine(resources.pc.getValue());
        resources.registers[registerIndex]
            .setInstruction(resources.instructionMemory[resources.pc.getValue()].getInstruction());
        resources.registers[registerIndex].available = false;
        resources.pc.setValue(resources.pc.getValue() + 1);
        return registerIndex;
    }
}