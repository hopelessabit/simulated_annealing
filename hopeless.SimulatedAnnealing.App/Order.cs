
class Order
{
    public string Name { get; }
    public List<Step> Steps{ get; set;  }
    public int Duration { get; }
    public int CurrentStep { get; set; }
    public int DelayedTime { get; set; }
    public int StartTime { get; set; }

    public Order(string name, List<Step> steps, int duration)
    {
        Name = name;
        Steps = steps;
        Duration = duration;
        CurrentStep = -1;
        DelayedTime = -1;
    }
    public void NextStep()
    {
        CurrentStep++;
    }
    public void MarkStepProcessing(int stationId)
    {
        Steps[CurrentStep].MarkProcessing(stationId);
    }
    public void MarkStepProcessingFirstTime(int stationId)
    {
        CurrentStep = 0;
        Steps[CurrentStep].MarkProcessing(stationId);
    }
    public void MarkComplete()
    {
        Steps[CurrentStep].MarkComplete();
        CurrentStep++;
    }
    public override string? ToString()
    {

        string a = $"{Name} - Duration: {Duration} - ";
        foreach(var t in Steps)
        {
            a += t.ToString() + ",";
        }
        return a;
    }

}