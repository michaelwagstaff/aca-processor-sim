using ProcessorSim.HardwareResources;
using ProcessorSim.Enums;
using ProcessorSim.Instructions;
using Monitor = System.Threading.Monitor;

namespace ProcessorSim;

public class Resources
{
    public bool verbose;
    public bool speculativeExecution;

    public Register[] registers;
    public Register[] vregisters;
    public Register pc;

    public MemorySlot[] instructionMemory;
    public MemorySlot[] dataMemory;
    
    public List<FetchUnit> fetchUnits;
    public BranchPredictor branchPredictor;
    public List<(int, (bool, (bool, int, int)))> instructionsWaitingDecode;
    public List<DecodeUnit> decodeUnits;
    public Dictionary<ExecutionTypes, ReservationStation> reservationStations;
    public Dictionary<ExecutionTypes, List<ExecutionUnit>> executionUnits;
    public ReOrderBuffer reorderBuffer;
    public List<Instruction> instructionsWaitingMemory;
    public MemoryUnit memoryUnit;
    public CommitUnit commitUnit;
    
    public HardwareResources.Monitor monitor;
    public Resources(int regCount, int instCount, int dataCount, bool verbose = false, int superscalarCount=1, bool specExEnabled = true)
    {
        this.verbose = verbose;
        speculativeExecution = specExEnabled;
        
        registers = new Register[regCount];
        for(int i = 0; i < registers.Length; i++)
        {
            registers[i] = new Register();
            registers[i].index = i;
        }
        pc = registers[0];
        vregisters = new Register[16];
        for(int i = 0; i < vregisters.Length; i++)
        {
            vregisters[i] = new Register(vector:true);
            vregisters[i].index = i;
        }
        
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
        branchPredictor = new BranchPredictor();
        instructionsWaitingDecode = new List<(int, (bool, (bool, int, int)))>();
        decodeUnits = new List<DecodeUnit>();
        for (int i = 0; i < superscalarCount; i++)
        {
            fetchUnits.Add(new FetchUnit());
            decodeUnits.Add(new DecodeUnit());
        }
        reservationStations = new Dictionary<ExecutionTypes, ReservationStation>();
        reservationStations[ExecutionTypes.General] = new ReservationStation(ExecutionTypes.General, 32, this);
        reservationStations[ExecutionTypes.SimpleArithmetic] = new ReservationStation(ExecutionTypes.SimpleArithmetic, 32, this);
        reservationStations[ExecutionTypes.ComplexArithmetic] = new ReservationStation(ExecutionTypes.ComplexArithmetic, 32, this);
        reservationStations[ExecutionTypes.LoadStore] = new ReservationStation(ExecutionTypes.LoadStore, 0, this);
        reservationStations[ExecutionTypes.Vector] = new ReservationStation(ExecutionTypes.Vector, 0, this);
        reservationStations[ExecutionTypes.Branch] = new ReservationStation(ExecutionTypes.Branch, 16, this);
        instructionsWaitingMemory = new List<Instruction>();
        memoryUnit = new MemoryUnit();
        commitUnit = new CommitUnit();
        reorderBuffer = new ReOrderBuffer(this);
    }

    public void setExecutionUnits(int generalExecutionUnits, int arithmeticUnits, int loadStoreUnits, int branchUnits, int vectorUnits)
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
        executionUnits.Add(ExecutionTypes.Vector, new List<ExecutionUnit>());
        for (int i = 0; i < vectorUnits; i++)
        {
            executionUnits[ExecutionTypes.Vector].Add(new ExecutionUnit(ExecutionTypes.Vector));
        }
    }

    public void CDBBroadcast(int reorderBufferSlot, int value)
    {
        foreach (ReservationStation reservationStation in reservationStations.Values)
        {
            reservationStation.CDBUpdate(reorderBufferSlot, value);
        }

        reorderBuffer.CDBUpdate(reorderBufferSlot, value);
    }
    public void CDBVectorBroadcast(int reorderBufferSlot, int[] value)
    {
        reservationStations[ExecutionTypes.Vector].CDBUpdate(reorderBufferSlot, value);

        reorderBuffer.CDBUpdate(reorderBufferSlot, value);
    }

    public void CDBBroadcastMemoryAddress(int reorderBufferSlot, int memoryAddress)
    {
        reorderBuffer.setMemoryAddress(reorderBufferSlot, memoryAddress);
    }
}