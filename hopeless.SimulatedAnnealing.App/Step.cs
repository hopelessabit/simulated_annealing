public class Step
{
    public double RequireTime { get; }
    public int StationProcess { get; set; }
    public double StartTime { get; set; }
    public double CompleteTime { get; set; }
    public StepStatus Status { get; set; }

    public Step(double requireTime)
    {
        RequireTime = requireTime;
        Status = StepStatus.UNPROCESSED;
    }

    public void MarkComplete(double completeTime)
    {
        Status = StepStatus.COMPLETED;
        CompleteTime = completeTime;
    }
    public void MarkProcessing(int stationId, double startTime)
    {
        Status = StepStatus.PROCESSING;
        StartTime = startTime;
        StationProcess = stationId;
    }
    public Step Clone()
    {
        return new Step(RequireTime);
    }
    public override string? ToString()
    {
        return $"Require time: {RequireTime} |\tStation process: {StationProcess} |\tStatus: {Status} |\tStart: {StartTime}\tCompelte: {CompleteTime}";
    }
}