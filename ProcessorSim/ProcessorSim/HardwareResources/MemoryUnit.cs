using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using ProcessorSim.Enums;

namespace ProcessorSim.HardwareResources;

public class MemoryUnit
{
    public bool memory(Resources resources)
    {
        Instruction instruction = resources.instructionWaitingMemory;
        if (instruction != null)
        {
            if (resources.verbose)
            {
                Console.WriteLine("Memory Access for Instruction {0}", instruction);
                Console.WriteLine("Register File {0}", instruction.registerFile);
                Console.WriteLine("Instruction to Execute: {0}", instruction);
                Console.WriteLine("Logical Output Register: {0}", instruction.targetRegister);
                Console.WriteLine("Actual Output Register: {0}",
                    resources.registerFile.getPhysicalRegister(instruction.registerFile, instruction.targetRegister));
            }
            Register actualTargetRegister =
                resources.registerFile.getPhysicalRegister(instruction.registerFile, instruction.targetRegister);
            if (instruction.executionType == ExecutionTypes.SimpleArithmetic)
            {
                if (resources.verbose)
                {
                    Console.WriteLine("Memory stage for arithmetic instruction {0}: saving result {1} to register file",
                        instruction, instruction.result);
                }

                if (instruction.targetRegister != null)
                {
                    Console.WriteLine(actualTargetRegister.getValue());
                    resources.forwardedResults[actualTargetRegister] = instruction.result;
                    resources.dataHazards[actualTargetRegister] = false;
                    resources.reservationStation.markRegisterUnblocked(actualTargetRegister);
                }
            }
            if (instruction.executionType == ExecutionTypes.LoadStore)
            {
                if (resources.verbose)
                {
                    Console.WriteLine("Memory stage for load/store instruction {0}",
                        instruction);
                }

                if (instruction.targetRegister != null)
                {
                    // i.e. we're doing a load
                    // Do loads get forwarded here or do we have to wait until writeback??
                    resources.forwardedResults[actualTargetRegister] = instruction.result;
                    resources.dataHazards[actualTargetRegister] = false;
                    resources.reservationStation.markRegisterUnblocked(actualTargetRegister);
                }
                // Else do nothing until writeback?
            }
            resources.instructionWaitingWriteback = instruction;
            resources.instructionWaitingMemory = null;
        }

        return true;
    }
}