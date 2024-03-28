using System.Collections.Generic;
using System.Linq;

public class Order
{
    public int Id { get; set; }
    public string Name { get; }
    public List<Step> Steps{ get; set;  }
    public double ExpectedDuration { get; }
    public int CurrentStep { get; set; }
    public double CompleteTime { get; set; }
    public double StartTime { get; set; }
    public OrderStatus Status { get; set; }

    public Order(int id, List<Step> steps, double duration)
    {
        Id = id;
        Steps = steps;
        ExpectedDuration = duration;
        CurrentStep = -1;
        StartTime = -1;
        CompleteTime = -1;
        Status = OrderStatus.PENDING;
    }
    public Order(int id, string name, List<Step> steps, double duration)
    {
        Id = id;
        Name = name;
        Steps = steps;
        ExpectedDuration = duration;
        CurrentStep = -1;
        StartTime = -1;
        CompleteTime = -1;
        Status = OrderStatus.PENDING;
    }
    public void NextStep()
    {
        CurrentStep++;
    }
    public void MarkStepProcessing(Station station, double currentTime)
    {
        CurrentStep++;
        if (Status.Equals(OrderStatus.PENDING))
        {
            StartTime = currentTime;
            Status = OrderStatus.PROCESSING;
        }
        Steps[CurrentStep].MarkProcessing(station.Id, currentTime);
        //Console.WriteLine($"=============           ORDER [{this.Id}] PROCESSING           =============\n{this.ToString()}");
    }
    public void MarkStepProcessingV2(Machine machine, double currentTime)
    {
        CurrentStep++;
        if (Status.Equals(OrderStatus.PENDING))
        {
            StartTime = currentTime;
            Status = OrderStatus.PROCESSING;
        }
        Steps[CurrentStep].MarkProcessing(machine.Id, currentTime);
        //Console.WriteLine($"=============           ORDER [{this.Id}] PROCESSING           =============\n{this.ToString()}");
    }
    public void MarkComplete(double currentTime)
    {
        Steps[CurrentStep].MarkComplete(currentTime);
        Status = OrderStatus.COMPLETE;
        CompleteTime = currentTime;
        //CurrentStep++;
        //Console.WriteLine($"=============           ORDER [{this.Id}] COMPLETE           =============\n{this.ToString()}");

    }
    public void MarkStepComplete(double completeTime)
    {
        Steps[CurrentStep].MarkComplete(completeTime);
    }
    public void MarkProcess(ref Machine machine, double currentTime)
    {
        CurrentStep++;
        if (CurrentStep == 0)
        {
            StartTime = currentTime;
            Status = OrderStatus.PROCESSING;
        }
        Steps[CurrentStep].MarkProcessing(machine, currentTime);
        machine.MarkProcessing(this);
    }
    public void MarkProcessComplete(double currentTime)
    {
        Steps[CurrentStep].MarkComplete(currentTime);
        if (CurrentStep == 5)
        {
            Status = OrderStatus.COMPLETE;
            CompleteTime = currentTime;
        }
    }
    public bool CanBeProcess()
    {
        return ((!this.Status.Equals(OrderStatus.COMPLETE)) && !this.Steps.Any(step => step.Status.Equals(StepStatus.PROCESSING)));
    }
    public Order Clone()
    {
        List<Step> clonedSteps = new List<Step>();
        Steps.ForEach(step => clonedSteps.Add(step.Clone()));
        return new Order(Id, Name, clonedSteps, ExpectedDuration);
    }

    public static Order CloneOrder(Order order)
    {
        List<Step> steps = new List<Step>();
        order.Steps.ForEach(step => steps.Add(Step.CloneStep(step)));
        return new Order(order.Id, order.Name, steps, order.ExpectedDuration);
    }
    public override string? ToString()
    {

        string a = $"{Id} - ExpectedDuration: {ExpectedDuration} - Current step: {CurrentStep} - Start Time: {StartTime} - Complete Time: {CompleteTime}\n";
        foreach(var t in Steps)
        {
            a += "\t" + t.ToString() + "\n";
        }
        return a;
    }

}