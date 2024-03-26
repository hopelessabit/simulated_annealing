using System;
using System.Collections.Generic;
using System.Linq;

public class SimulatedAnnealing
{
    public List<Order> orders;
    private double initialTemperature;
    private double coolingRate;
    public static int MachinesPerStation = 2;
    public static Random random = new Random();
    public SimulatedAnnealing(List<Order> orders, double initialTemperature, double coolingRate, int machinesPerStation)
    {
        this.orders = orders;
        this.initialTemperature = initialTemperature;
        this.coolingRate = coolingRate;
        MachinesPerStation = machinesPerStation;
    }

    public List<Order> FindBestSolution()
    {
        List<Order> currentSolution = GenerateRandomOrder();
        List<Order> bestSolution = new List<Order>(CloneListOrder(currentSolution));

        double temperature = initialTemperature;

        while (temperature > 1)
        {
            List<Order> newSolution = GenerateNeighbor(CloneListOrder(currentSolution));

            double currentEnergy = CalculateDelayedTimeV2(currentSolution);
            double neighborEnergy = CalculateDelayedTimeV2(newSolution);

            double acceptanceProbability = AcceptanceProbability(currentEnergy, neighborEnergy, temperature);

            if (acceptanceProbability > random.NextDouble())
            {
                currentSolution = new List<Order>(newSolution);
            }

            if (CalculateDelayedTimeV2(currentSolution) < CalculateDelayedTimeV2(bestSolution))
            {
                bestSolution = new List<Order>(currentSolution);
            }

            temperature *= 1 - coolingRate;
        }
        return bestSolution;
    }

    private List<Order> GenerateRandomOrder()
    {
        return ShuffleOrders(CloneListOrder(orders));
    }
    private List<Order> GenerateNeighbor(List<Order> currentSolution)
    {
        List<Order> neighborSolution = new List<Order>(currentSolution);
        int index1 = random.Next(0, currentSolution.Count);
        int index2 = random.Next(0, currentSolution.Count);

        Order temp = neighborSolution[index1];
        neighborSolution[index1] = neighborSolution[index2];
        neighborSolution[index2] = temp;

        return neighborSolution;
    }
    private double AcceptanceProbability(double currentEnergy, double neighborEnergy, double temperature)
    {
        if (neighborEnergy < currentEnergy)
        {
            return 1.0;
        }
        return Math.Exp((currentEnergy - neighborEnergy) / temperature);
    }
    private static void ProcessOrder(ref List<Order> orders, ref List<Station> stations, double currentTime)
    {
        //Console.Clear();
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
                    //Console.WriteLine($"\n\n\n==============     ORDERING     ==============\n{avaiableStation.ToString()}\n" +
                    //    $"{order.ToString()}");
                    order.MarkStepProcessing(avaiableStation, currentTime);
                    avaiableStation.MarkProcessing(order);
                    //Console.WriteLine($"\n\n\n==============     AFTER ORDERING     ==============\n{avaiableStation.ToString()}\n" +
                    //    $"{order.ToString()}");
                }
            }
        }
    }
    private static void ProcessOrderV2(ref List<Order> orders, ref List<Station> stations, double currentTime)
    {
        if (!ThereIsStationAvaiable(stations))
            return;
        Machine? avaiableMachine;
        foreach (Order order in orders)
        {
            if (order.Status.Equals(OrderStatus.COMPLETE) || order.Steps.Any(step => step.Status.Equals(StepStatus.PROCESSING)))
                continue;
            else
            {

                avaiableMachine = null;
                avaiableMachine = GetMachineAvaiableV3(order, stations);
                if (avaiableMachine != null)
                {
                    //Console.WriteLine($"\n\n\n==============     ORDERING     ==============\n{avaiableStation.ToString()}\n" +
                    //    $"{order.ToString()}");
                    order.MarkStepProcessingV2(avaiableMachine, currentTime);
                    stations[order.CurrentStep].MarkProcessingV2(order);
                    //Console.WriteLine($"\n\n\n==============     AFTER ORDERING     ==============\n{avaiableStation.ToString()}\n" +
                    //    $"{order.ToString()}");
                }
            }
        }
    }
    public static double CalculateDelayedTimeV2(List<Order> orders)
    {
        List<Station> stations = new List<Station>();
        for (int i = 0; i < orders[0].Steps.Count; i++)
        {
            stations.Add(new Station(i, MachinesPerStation));
        }

        double currentTime = 0;
        //Console.Write("Order: "); orders.ForEach(order => Console.Write(order.Id + "\t"));
        while (!AllOrderComplete(orders))
        {
            //Console.Clear();
            ProcessOrderV2(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
        }

        double delayTime = 0;
        Console.WriteLine("Orders: ");
        orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                delayTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
            Console.Write($"{order.Id}: {order.StartTime} -> {order.CompleteTime} => {order.CompleteTime - order.StartTime - order.ExpectedDuration}\n");
        });
        Console.WriteLine($"\t\t DELAY TIME :{delayTime}");
        return delayTime;
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
        //Console.WriteLine($"Smallest remaining time: {smallestRemainingTime}");
        //stations.ForEach(station => Console.WriteLine(station.ToString()));
        foreach (Station station in stations)
        {
            foreach (Machine machine in station.Machines)
            {
                if (machine.RemainingTime == -1)
                    continue;
                else if (machine.RemainingTime == smallestRemainingTime)
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
                    //Console.WriteLine($" ==============     AFTER PROCESSING     ==============\nStation {station.Id}:{machine.ToString()}\n" +
                    //    $"{order.ToString()}");
                    if (order.CurrentStep == 3)
                    {
                        order.MarkComplete(currentTime);
                    }
                    else
                        order.MarkStepComplete(currentTime);
                }
                else
                {
                    machine.RemainingTime -= smallestRemainingTime;
                    //Console.WriteLine($" ==============     AFTER PROCESSING     ==============\nStation {station.Id}:{machine.ToString()}\n");
                }
            }
        }
        currentTime += smallestRemainingTime;
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
        //Console.Write("Order: "); orders.ForEach(order => Console.Write(order.Id + "\t"));
        while (!AllOrderComplete(orders))
        {
            //Console.Clear();
            ProcessOrder(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
            currentTime = Processing(ref orders, ref stations, currentTime);
            //Print(orders, stations, currentTime);
        }

        double delayTime = 0;
        Console.WriteLine("Orders: ");
        orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                delayTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
            Console.Write($"{order.Id}: {order.StartTime} -> {order.CompleteTime} => {order.CompleteTime - order.StartTime - order.ExpectedDuration}\n");
        });
        Console.WriteLine($"\t\t DELAY TIME :{delayTime}");
        return delayTime;
    }
    private static List<Order> GenerateNewOrder()
    {
        return new List<Order>
        {
            new Order(1, new List<Step>() { new Step(9), new Step(1), new Step(0), new Step(3) }, 7),
            new Order(2, new List<Step>() { new Step(0), new Step(2), new Step(6), new Step(4) }, 13),
            new Order(3, new List<Step>() { new Step(7), new Step(4), new Step(3), new Step(0) }, 17),
            new Order(4, new List<Step>() { new Step(5), new Step(2), new Step(0), new Step(3) }, 10),
            new Order(5, new List<Step>() { new Step(0), new Step(2), new Step(12), new Step(4) }, 14),
            new Order(6, new List<Step>() { new Step(4), new Step(8), new Step(3), new Step(0) }, 10),
            new Order(0, new List<Step>() { new Step(10), new Step(7), new Step(4), new Step(5) }, 10),
        };
    }
    private static bool AllOrderComplete(List<Order> orders)
    {
        return orders.All(order => order.Status.Equals(OrderStatus.COMPLETE));
    }
    private static void Print(List<Order> orders, List<Station> stations, int currentTime)
    {
        Console.WriteLine($"\n\n\n---------=          [[[[{currentTime}]]]]          =---------");
        orders.ForEach(order =>
        {
            Console.WriteLine(order.ToString()); ;
        });
        stations.ForEach(station =>
        {
            Console.WriteLine(station.ToString()); ;
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
            if (station.Machines.Any(machine => machine.Id == (order.CurrentStep + 1) && machine.Status.Equals(MachineStatus.UNPROCESSING)))
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
            if (station.Id == (order.CurrentStep + 1) && station.Machines.Any(machine => machine.Status.Equals(MachineStatus.UNPROCESSING)))
                return station.Machines.FirstOrDefault(machine => machine.Status.Equals(MachineStatus.UNPROCESSING));
        }
        return null;
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
    private static List<Order> CloneListOrder(List<Order> orders)
    {
        List<Order> result = new List<Order>();
        orders.ForEach(order => result.Add(order.Clone()));
        return result;
    }
}