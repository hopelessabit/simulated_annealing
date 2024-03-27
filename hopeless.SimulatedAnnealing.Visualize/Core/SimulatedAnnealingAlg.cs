using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class SimulatedAnnealingAlg
{
    private List<Order> orders;
    private double initialTemperature;
    private double coolingRate;
    public static int MachinesPerStation = 5;
    public static Random random = new Random();
    public SimulatedAnnealingAlg(List<Order> orders, double initialTemperature, double coolingRate, int machinesPerStation)
    {
        this.orders = orders;
        this.initialTemperature = initialTemperature;
        this.coolingRate = coolingRate;
        MachinesPerStation = machinesPerStation;
    }

    public List<Order> FindBestSolution()
    {
        List<Order> currentSolution = orders;
        List<Order> bestSolution = CloneListOrder(currentSolution);

        double temperature = initialTemperature;

        while (temperature > 1)
        {
            List<Order> newSolution = GenerateNeighbor(CloneListOrder(currentSolution));
            double currentEnergy = CalculateDelayedTimeV2(currentSolution);
            double neighborEnergy = CalculateDelayedTimeV2(newSolution);
            double acceptanceProbability = AcceptanceProbability(currentEnergy, neighborEnergy, temperature);

            if (acceptanceProbability > random.NextDouble())
            {
                currentSolution = new List<Order>(CloneListOrder(newSolution));
            }

            if (CalculateDelayedTimeV2(currentSolution) < CalculateDelayedTimeV2(newSolution))
            {
                bestSolution = new List<Order>(CloneListOrder(currentSolution));
            }

            temperature *= 1 - coolingRate;
        }
        return bestSolution;
    }

    public static List<Order> ProcessOrder(List<Order> orders)
    {
        List<Station> stations = new List<Station>();
        for (int i = 0; i < orders[0].Steps.Count; i++)
        {
            stations.Add(new Station(i, MachinesPerStation));
        }

        double currentTime = 0;
        while (!AllOrderComplete(orders))
        {
            MarkProcessOrderV2(ref orders, ref stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
        }

        double delayTime = 0;
        //Debug.WriteLine("Orders: ");
        orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                delayTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
            //Debug.Write($"{order.Id}: {order.StartTime} -> {order.CompleteTime} => {order.CompleteTime - order.StartTime - order.ExpectedDuration}\n");
        });
        //Debug.WriteLine($"\t\t DELAY TIME :{delayTime}");
        return orders;
    }

    public List<Order> GenerateRandomOrder()
    {
        return ShuffleOrders(CloneListOrder(orders));
    }
    private List<Order> GenerateNeighbor(List<Order> currentSolution)
    {
        List<Order> neighborSolution = new List<Order>(currentSolution);
        int index1 = random.Next(0, currentSolution.Count);
        int index2 = random.Next(0, currentSolution.Count);
        Debug.WriteLine($"SWAP: {index1} <-> {index2}");
        Order temp = neighborSolution[index1];
        neighborSolution[index1] = neighborSolution[index2];
        neighborSolution[index2] = temp;

        return neighborSolution;
    }
    private double AcceptanceProbability(double currentEnergy, double neighborEnergy, double temperature)
    {
        if (neighborEnergy < currentEnergy)
        {
            return 1.0; //Always accept better solution
        }
        return Math.Exp((currentEnergy - neighborEnergy) / temperature); // Accept worse solutions with a probability based on temperature
    }
    private static void ProcessOrder(ref List<Order> orders, ref List<Station> stations, double currentTime)
    {
        //Debug.Clear();
        Station? avaiableStation = null;
        if (!ThereIsStationAvaiable(stations))
            return;
        foreach (Order order in orders)
        {
            if (order.Status.Equals(OrderStatus.COMPLETE) || order.Steps.Any(step => step.Status.Equals(StepStatus.PROCESSING)))
                continue;
            else
            {
                avaiableStation = null;
                avaiableStation = GetStationGotMachineAvaiableV2(order, stations);
                if (avaiableStation != null)
                {
                    //Debug.WriteLine($"\n\n\n==============     ORDERING     ==============\n{avaiableStation.ToString()}\n" +
                    //    $"{order.ToString()}");
                    order.MarkStepProcessing(avaiableStation, currentTime);
                    avaiableStation.MarkProcessing(order);
                    //Debug.WriteLine($"\n\n\n==============     AFTER ORDERING     ==============\n{avaiableStation.ToString()}\n" +
                    //    $"{order.ToString()}");
                }
            }
        }
    }
    private static void MarkProcessOrderV2(ref List<Order> orders, ref List<Station> stations, double currentTime)
    {
        if (!ThereIsStationAvaiable(stations))
            return;
        Machine? avaiableMachine;
        foreach (Order order in orders)
        {
            if (order.Status.Equals(OrderStatus.COMPLETE) || order.Steps.Any(step => step.Status.Equals(StepStatus.PROCESSING)))
                continue;

            avaiableMachine = null;
            avaiableMachine = GetMachineAvaiableV3(order, stations);
            if (avaiableMachine != null)
            {
                order.MarkStepProcessingV2(avaiableMachine, currentTime);
                avaiableMachine.MarkProcessing(order);
            }
        }
    }
    public static double CalculateDelayedTimeV2(List<Order> orders)
    {
        //Debug.Write("\nOrder: \t");
        //orders.ForEach(order => Debug.Write(order.Id +"\t"));
        List<Station> stations = new List<Station>();
        for (int i = 0; i < orders[0].Steps.Count; i++)
        {
            stations.Add(new Station(i, MachinesPerStation));
        }

        double currentTime = 0;
        while (!AllOrderComplete(orders))
        {
            MarkProcessOrderV2(ref orders, ref stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
        }
        double delayTime = 0;
        //Debug.WriteLine("");
        foreach (Order o in orders)
        {
            double cal = o.CompleteTime - o.StartTime - o.ExpectedDuration;
            if (cal > 0)
            {
                delayTime += cal;
                //Debug.Write($"{cal} + ");
            }
        }
        //Debug.WriteLine("");

        //orders.ForEach(order =>
        //{
        //    Debug.Write(order.ToString());
        //});

        Debug.WriteLine("=========================================== DELTA T: " + delayTime + " === TOTAL: " + orders.OrderByDescending(order => order.CompleteTime).FirstOrDefault().CompleteTime);
        //orders.ForEach(order =>
        //{
        //    if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
        //        delayTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
        //});
        return delayTime;
    }
    public static double GetDeltaT(List<Order> orders)
    {
        double delayTime = 0;
        orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                delayTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
        });
        return delayTime;
    }
    public static double CalculateProcessingTime(List<Order> orders)
    {
        List<Station> stations = new List<Station>();
        for (int i = 0; i < orders[0].Steps.Count; i++)
        {
            stations.Add(new Station(i, MachinesPerStation));
        }

        double currentTime = 0;
        //Debug.Write("Order: "); orders.ForEach(order => Debug.Write(order.Id + "\t"));
        while (!AllOrderComplete(orders))
        {
            //Debug.Clear();
            MarkProcessOrderV2(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
        }
        return currentTime;
    }
    private static double Processing(ref List<Order> orders, ref List<Station> stations, double currentTime)
    {
        double smallestRemainingTime = double.MaxValue;
        foreach (Station station in stations)
        {
            foreach (Machine machine in station.Machines)
            {
                if (machine.Status.Equals(MachineStatus.PROCESSING) && machine.RemainingTime != -1 && smallestRemainingTime > machine.RemainingTime)
                    smallestRemainingTime = machine.RemainingTime;
            }
        }
        foreach (Station station in stations)
        {
            foreach (Machine machine in station.Machines)
            {
                if (machine.RemainingTime == -1)
                    continue;
                if (machine.RemainingTime != smallestRemainingTime)
                {
                    machine.RemainingTime -= smallestRemainingTime;
                } 
                else
                {
                    Order order = null;
                    foreach (Order o in orders)
                    {
                        if (o.Id == machine.OrderId)
                            order = o;
                    }
                    if (order == null)
                        continue;
                    machine.MarkComplete();
                    if (order.CurrentStep == 5)
                    {
                        order.MarkComplete(currentTime + smallestRemainingTime);
                    }
                    else
                        order.MarkStepComplete(currentTime + smallestRemainingTime);
                }
            }
            currentTime += smallestRemainingTime;
        }
        return currentTime;
    }
    public static double CalculateDeplayedTime(List<Order> orders)
    {
        List<Station> stations = new List<Station>();
        for (int i = 0; i < 5; i++)
        {
            stations.Add(new Station(i, orders[0].Steps.Count));
        }

        double currentTime = 0;
        //Debug.Write("Order: "); orders.ForEach(order => Debug.Write(order.Id + "\t"));
        while (!AllOrderComplete(orders))
        {
            //Debug.Clear();
            ProcessOrder(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
        }

        double delayTime = 0;
        Debug.WriteLine("Orders: ");
        orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                delayTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
            Debug.Write($"{order.Id}: {order.StartTime} -> {order.CompleteTime} => {order.CompleteTime - order.StartTime - order.ExpectedDuration}\n");
        });
        Debug.WriteLine($"\t\t DELAY TIME :{delayTime}");
        return delayTime;
    }
    public List<Order> TheProcesser(List<Order> orders)
    {
        List<Station> stations = new List<Station>();
        for (int i = 0; i < orders[0].Steps.Count; i++)
        {
            stations.Add(new Station(i, MachinesPerStation));
        }

        double currentTime = 0;
        //Debug.Write("Order: "); orders.ForEach(order => Debug.Write(order.Id + "\t"));
        while (!AllOrderComplete(orders))
        {
            //Debug.Clear();
            MarkProcessOrderV2(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
        }
        return orders;
    }
    private static bool AllOrderComplete(List<Order> orders)
    {
        return orders.All(order => order.Status.Equals(OrderStatus.COMPLETE));
    }
    private static void Print(List<Order> orders, List<Station> stations, int currentTime)
    {
        Debug.WriteLine($"\n\n\n---------=          [[[[{currentTime}]]]]          =---------");
        orders.ForEach(order =>
        {
            Debug.WriteLine(order.ToString()); ;
        });
        stations.ForEach(station =>
        {
            Debug.WriteLine(station.ToString()); ;
        });
    }
    private static bool ThereIsStationAvaiable(List<Station> stations)
    {
        int avaiable = 0;
        foreach (Station station in stations)
        {
            if (station.AreAnyMachineAvaiable())
            {
                avaiable++;
            }
        }
        return avaiable != 0 ? true : false;
    }
    private static Station? GetStationGotMachineAvaiableV2(Order order, List<Station> stations)
    {
        Station result = null;
        foreach (Station station in stations)
        {
            if (station.Machines.Any(machine => machine.Id == (order.CurrentStep + 1) && machine.Status.Equals(MachineStatus.NONPROCESSING)))
            {
                result = station;
                break;
            }
        }
        return result;
    }
    private static Machine? GetMachineAvaiableV3(Order order, List<Station> stations)
    {
        foreach (Station station in stations)
        {
            if (station.Id == (order.CurrentStep + 1) && CheckAvaiableMachineInStation(order, station))
                return station.Machines.FirstOrDefault(machine => machine.Status.Equals(MachineStatus.NONPROCESSING));
        }
        return null;
    }
    private static bool CheckAvaiableMachineInStation(Order order, Station station)
    {
        return station.Machines.Any(machine => machine.Status.Equals(MachineStatus.NONPROCESSING));
    }
    private static List<Order> ShuffleOrders(List<Order> orders)
    {
        // use fisher-yates shuffle algorithm to shuffle the orders
        List<Order> result = orders;
        int n = result.Count;
        while (n > 1)
        {
            n--;
            int k = new Random().Next(n + 1);
            Order temp = result[k];
            result[k] = result[n];
            result[n] = temp;
        }
        return result;
    }
    public static List<Order> CloneListOrder(List<Order> orders)
    {
        List<Order> result = new List<Order>();
        orders.ForEach(order => result.Add(order.Clone()));
        return result;
    }
}