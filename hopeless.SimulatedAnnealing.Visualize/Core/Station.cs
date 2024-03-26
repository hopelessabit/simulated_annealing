using System.Collections.Generic;
using System.Linq;

public class Station
{
    public int Id { get; }
    public string Name { get; }
    public List<Machine> Machines { get; set; }

    public Station(int id, int numMachines)
    {
        Id = id;
        Machines = new List<Machine>();
        for (int i = 0; i < numMachines; i++)
            Machines.Add(new Machine(i));
    }
    public bool AreAnyMachineAvaiable()
    {
        return Machines.FirstOrDefault(machine => machine.Status.Equals(MachineStatus.UNPROCESSING)) == null ? false : true;
    }
    public void MarkProcessing(Order order)
    {
        Machines.First(machine => machine.Id == order.CurrentStep)
            .MarkProcessing(order);
        //Console.WriteLine($"=============           STATION [{this.Id}] PROCESSING           =============\n{this.ToString()}");
    }
    public void MarkProcessingV2(Order order)
    {
        Machines.First(machine => machine.Status.Equals(MachineStatus.UNPROCESSING))
            .MarkProcessing(order);
        //Console.WriteLine($"=============           STATION [{this.Id}] PROCESSING           =============\n{this.ToString()}");
    }
    public void MarkComplete(Order order)
    {
        Machines[order.CurrentStep].MarkComplete();
        //Console.WriteLine($"=============           STATION [{this.Id}] COMPLETE           =============\n{this.ToString()}");
    }
    public void MarkComplete(Machine machine)
    {
        machine.MarkComplete();
    }
    public override string ToString()
    {
        string str = $"Station Id: {Id} \n";
        foreach (Machine machine in Machines)
        {
            str += $"\t{machine}\n";
        }
        return $"{Name} - {str}";
    }
}