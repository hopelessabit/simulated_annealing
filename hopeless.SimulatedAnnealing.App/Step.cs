public class Step
{
    public int RequireTime { get; }
    public int StationProcess { get; set; }
    public StepStatus Status { get; set; }

    public Step(int requireTime)
    {
        RequireTime = requireTime;
        Status = StepStatus.UNPROCESSED;
    }

    public void MarkComplete()
    {
        Status = StepStatus.PROCESSED;
        StationProcess = -1;
    }
    public void MarkProcessing(int stationId)
    {
        Status = StepStatus.PROCESSING;
        StationProcess = stationId;
    }
}