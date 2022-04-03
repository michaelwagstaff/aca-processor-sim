using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class MemoryUnit
{
    public bool memory(Resources resources, Instruction instruction)
    {
        if (instruction != null)
        {
            if (resources.verbose)
            {
                Console.WriteLine("  Instruction {0}", instruction);
                Console.WriteLine("  Register File {0}", instruction.registerFile);
                Console.WriteLine("  Actual Output Register: {0}", instruction.targetRegister);
                //Console.WriteLine("Actual Output Register: {0}",
                //    resources.registerFile.getPhysicalRegister(instruction.registerFile, instruction.targetRegister));
            }
            //Register actualTargetRegister =
            //    resources.registerFile.getPhysicalRegister(instruction.registerFile, instruction.targetRegister);
            if (instruction.executionType == ExecutionTypes.SimpleArithmetic)
            {
                /*
                if (resources.verbose)
                {
                    Console.WriteLine("Memory stage for arithmetic instruction {0}: saving result {1} to register file",
                        instruction, instruction.result);
                }
                */

                if (instruction.targetRegister != null)
                {
                    resources.forwardedResults[instruction.targetRegister] = instruction.result;
                    resources.registerInstructionsInFlight[instruction.targetRegister]--;
                    resources.reservationStation.markRegisterUnblocked(instruction.targetRegister);
                }
            }
            else if (instruction.executionType == ExecutionTypes.ComplexArithmetic)
            {
                resources.registerInstructionsInFlight[instruction.targetRegister]--;
                resources.reservationStation.markRegisterUnblocked(instruction.targetRegister);
            }
            else if (instruction.executionType == ExecutionTypes.LoadStore)
            {
                if (resources.verbose)
                {
                    /*
                    Console.WriteLine("Memory stage for load/store instruction {0}",
                        instruction);
                        */
                }

                if (instruction.targetRegister != null)
                {
                    // i.e. we're doing a load
                    // Do loads get forwarded here or do we have to wait until writeback??
                    resources.forwardedResults[instruction.targetRegister] = instruction.result;
                    resources.registerInstructionsInFlight[instruction.targetRegister] --;
                    resources.reservationStation.markRegisterUnblocked(instruction.targetRegister);
                }
                // Else do nothing until writeback?
            }
            resources.instructionsWaitingWriteback.Add(instruction);
            resources.instructionsWaitingMemory.Remove(instruction);
        }

        return true;
    }
}