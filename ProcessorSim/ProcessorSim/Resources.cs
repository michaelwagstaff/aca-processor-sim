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
    public List<DecodeUnit> decodeUnits;
    public ReservationStation reservationStation;
    public Dictionary<ExecutionTypes, List<ExecutionUnit>> executionUnits;
    public Instruction instructionWaitingMemory;
    public MemoryUnit memoryUnit;
    public Dictionary<Register, int?> forwardedResults;
    public Instruction instructionWaitingWriteback;
    public WritebackUnit writebackUnit;
    
    public HardwareResources.Monitor monitor;
    public Resources(int regCount, int instCount, int dataCount, bool verbose = false, int superscalarCount=1)
    {
        this.verbose = verbose;
        registers = new Register[regCount];
        for(int i = 0; i < registers.Length; i++)
        {
            registers[i] = new Register();
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
        decodeUnits = new List<DecodeUnit>();
        reservationStation = new ReservationStation(ExecutionTypes.General, 1);
        instructionWaitingMemory = null;
        forwardedResults = new Dictionary<Register, int?>();
        memoryUnit = new MemoryUnit();
        writebackUnit = new WritebackUnit();
        for (int i = 0; i < regCount; i++)
        {
            forwardedResults[registers[i]] = null;
        }
        instructionWaitingWriteback = null;
        for (int i = 0; i < superscalarCount; i++)
        {
            fetchUnits.Add(new FetchUnit());
            decodeUnits.Add(new DecodeUnit());
        }
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
        for (int i = 0; i < generalExecutionUnits; i++)
        {
            executionUnits[ExecutionTypes.SimpleArithmetic].Add(new ExecutionUnit(ExecutionTypes.SimpleArithmetic));
        }
        executionUnits.Add(ExecutionTypes.LoadStore, new List<ExecutionUnit>());
        for (int i = 0; i < generalExecutionUnits; i++)
        {
            executionUnits[ExecutionTypes.LoadStore].Add(new ExecutionUnit(ExecutionTypes.LoadStore));
        }
        executionUnits.Add(ExecutionTypes.Branch, new List<ExecutionUnit>());
        for (int i = 0; i < generalExecutionUnits; i++)
        {
            executionUnits[ExecutionTypes.Branch].Add(new ExecutionUnit(ExecutionTypes.Branch));
        }
    }
}