class Station
{
    public string Name { get; }
    public List<Machine> Machines { get; set; }

    public Station(string name, int numMachines)
    {
        Name = name;
        Machines = new List<Machine>();
        for (int i = 0; i < 4; i++)
            Machines.Add(new Machine());
    }

    public void MarkProcessing(int orderId, Order order)
    {
        Machines[order.CurrentStep].MarkProcessing(orderId, order.Steps[order.CurrentStep].RequireTime);
    }
    public void MarkComplete(Order order)
    {
        Machines[order.CurrentStep].MarkComplete();
    }
    public override string ToString()
    {
        string machineStatus = "";
        return $"{Name} Machines: {machineStatus}";
    }
}