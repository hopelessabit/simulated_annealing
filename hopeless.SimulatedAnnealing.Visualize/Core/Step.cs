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
        StartTime = -1;
        CompleteTime = -1;
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
        CompleteTime = -1;
        MachineProcess = machineId;
    }
    public void MarkProcessing(Machine machine, double currentTime)
    {
        MachineProcess = machine.Id;
        StartTime = currentTime;
        Status = StepStatus.PROCESSING;
    }
    public Step Clone()
    {
        return new Step(RequireTime, StationProcess);
    }

    public static Step CloneStep(Step step)
    {
        return new Step(step.RequireTime, step.StationProcess);
    }
    public override string? ToString()
    {
        //return $"Require time: {RequireTime} |\tStation process: {StationProcess} |\tStatus: {Status} |\tStart: {StartTime}\tCompelte: {CompleteTime}";
        //return $"Station: {StationProcess}, Machine: {MachineProcess}, Start: {StartTime} -> End: {CompleteTime} : Require Time: {RequireTime}";
        return $"Start: {StartTime.ToString("F2")} -> End: {CompleteTime.ToString("F2")} : Require Time: {RequireTime.ToString("F2")}";
    }
}