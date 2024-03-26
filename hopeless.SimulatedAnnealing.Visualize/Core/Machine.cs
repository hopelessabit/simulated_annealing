public class Machine
{
    public int Id { get; }
    public MachineStatus Status { get; set; }
    public double RemainingTime { get; set; }
    public int OrderId { get; set; }
    public Machine(int id)
    {
        Id = id;
        Status = MachineStatus.UNPROCESSING;
        RemainingTime = -1;
        OrderId = -1;
    }
    public void MarkProcessing(Order order)
    {
        Status = MachineStatus.PROCESSING;
        OrderId = order.Id;
        RemainingTime = order.Steps[order.CurrentStep].RequireTime;
    }
    public void MarkComplete()
    {   
        Status = MachineStatus.UNPROCESSING;
        RemainingTime = -1;
        OrderId = -1;
    }

    public override string? ToString()
    {
        return $"Machine Id: {Id}, Order Id: {OrderId}, Remaining Time: {RemainingTime}";
    }
}