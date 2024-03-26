public class Step
{
    public double RequireTime { get; }
    public int StationProcess { get; set; }
    public int MachineProcess { get; set; }
    public double StartTime { get; set; }
    public double CompleteTime { get; set; }
    public StepStatus Status { get; set; }

    public Step(double requireTime,int id)
    {
        StationProcess = id;
        MachineProcess = -1;
        RequireTime = requireTime;
        Status = StepStatus.UNPROCESSED;
    }

    public void MarkComplete(double completeTime)
    {
        CompleteTime = completeTime;
        Status = StepStatus.COMPLETED;
    }
    public void MarkProcessing(int machineId, double startTime)
    {
        Status = StepStatus.PROCESSING;
        StartTime = startTime;
        CompleteTime = startTime + RequireTime;
        MachineProcess = machineId;
    }
    public Step Clone()
    {
        return new Step(RequireTime, StationProcess);
    }
    public override string? ToString()
    {
        //return $"Require time: {RequireTime} |\tStation process: {StationProcess} |\tStatus: {Status} |\tStart: {StartTime}\tCompelte: {CompleteTime}";
        return $"Station: {StationProcess}, Machine: {MachineProcess}, Start: {StartTime} -> End: {CompleteTime} : Require Time: {RequireTime}";
    }
}