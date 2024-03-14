public class Machine
{
    public MachineStatus Status { get; set; }
    public int RemainingTime { get; set; }
    public int OrderId { get; set; }
    public Machine()
    {
        Status = MachineStatus.UNPROCESSING;
        RemainingTime = 0;
        OrderId = -1;
    }
    public void MarkProcessing(int orderId, int requireTime)
    {
        Status = MachineStatus.PROCESSING;
        OrderId = orderId;
        RemainingTime = requireTime;
    }
    public void MarkComplete()
    {
        Status = MachineStatus.UNPROCESSING;
        RemainingTime = 0;
        OrderId = -1;
    }
}