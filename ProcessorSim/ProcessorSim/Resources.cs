using ProcessorSim.HardwareResources;
using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using Monitor = System.Threading.Monitor;

namespace ProcessorSim;

public class Resources
{
    public bool verbose;
    
    public Register[] registers;
    public Register pc;

    public MemorySlot[] instructionMemory;
    public MemorySlot[] dataMemory;
    
    public List<FetchUnit> fetchUnits;
    public List<(int, (bool, bool))> instructionsWaitingDecode;
    public List<DecodeUnit> decodeUnits;
    public ReservationStation reservationStation;
    public Dictionary<ExecutionTypes, List<ExecutionUnit>> executionUnits;
    public Dictionary<Register, int> registerInstructionsInFlight;
    public RegisterFile registerFile;
    public List<Instruction> instructionsWaitingMemory;
    public MemoryUnit memoryUnit;
    public Dictionary<Register, int?> forwardedResults;
    public List<Instruction> instructionsWaitingWriteback;
    public WritebackUnit writebackUnit;
    
    public HardwareResources.Monitor monitor;
    public Resources(int regCount, int instCount, int dataCount, bool verbose = false, int superscalarCount=1)
    {
        this.verbose = verbose;
        registers = new Register[regCount];
        for(int i = 0; i < registers.Length; i++)
        {
            registers[i] = new Register();
            registers[i].index = i;
        }
        pc = registers[0];
        
        instructionMemory = new MemorySlot[instCount];
        for(int i = 0; i < instructionMemory.Length; i++)
        {
            instructionMemory[i] = new MemorySlot();
        }
        
        dataMemory = new MemorySlot[dataCount];
        for(int i = 0; i < dataMemory.Length; i++)
        {
            dataMemory[i] = new MemorySlot();
        }

        monitor = new HardwareResources.Monitor();
        fetchUnits = new List<FetchUnit>();
        instructionsWaitingDecode = new List<(int, (bool, bool))>();
        decodeUnits = new List<DecodeUnit>();
        for (int i = 0; i < superscalarCount; i++)
        {
            fetchUnits.Add(new FetchUnit());
            decodeUnits.Add(new DecodeUnit());
        }
        registerInstructionsInFlight = new Dictionary<Register, int>();
        registerFile = new RegisterFile(this);
        Register[] physicalRegisters = registerFile.getPhysicalRegisters();
        reservationStation = new ReservationStation(ExecutionTypes.General, 16);
        instructionsWaitingMemory = new List<Instruction>();
        forwardedResults = new Dictionary<Register, int?>();
        memoryUnit = new MemoryUnit();
        writebackUnit = new WritebackUnit();
        for (int i = 0; i < physicalRegisters.Length; i++)
        {
            registerInstructionsInFlight[physicalRegisters[i]] = 0;
            forwardedResults[physicalRegisters[i]] = null;
        }

        instructionsWaitingWriteback = new List<Instruction>();
    }

    public void setExecutionUnits(int generalExecutionUnits, int arithmeticUnits, int loadStoreUnits, int branchUnits)
    {
        executionUnits = new Dictionary<ExecutionTypes, List<ExecutionUnit>>();
        executionUnits.Add(ExecutionTypes.General, new List<ExecutionUnit>());
        for (int i = 0; i < generalExecutionUnits; i++)
        {
            executionUnits[ExecutionTypes.General].Add(new ExecutionUnit(ExecutionTypes.General));
        }
        executionUnits.Add(ExecutionTypes.SimpleArithmetic, new List<ExecutionUnit>());
        for (int i = 0; i < arithmeticUnits; i++)
        {
            executionUnits[ExecutionTypes.SimpleArithmetic].Add(new ExecutionUnit(ExecutionTypes.SimpleArithmetic));
        }
        executionUnits.Add(ExecutionTypes.ComplexArithmetic, new List<ExecutionUnit>());
        for (int i = 0; i < generalExecutionUnits; i++)
        {
            executionUnits[ExecutionTypes.ComplexArithmetic].Add(new ExecutionUnit(ExecutionTypes.ComplexArithmetic));
        }
        executionUnits.Add(ExecutionTypes.LoadStore, new List<ExecutionUnit>());
        for (int i = 0; i < loadStoreUnits; i++)
        {
            executionUnits[ExecutionTypes.LoadStore].Add(new ExecutionUnit(ExecutionTypes.LoadStore));
        }
        executionUnits.Add(ExecutionTypes.Branch, new List<ExecutionUnit>());
        for (int i = 0; i < branchUnits; i++)
        {
            executionUnits[ExecutionTypes.Branch].Add(new ExecutionUnit(ExecutionTypes.Branch));
        }
    }
}