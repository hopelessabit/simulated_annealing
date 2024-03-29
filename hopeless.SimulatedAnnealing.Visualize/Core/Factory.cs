using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class Factory
{
    public List<Order> Orders { get; set; }
    public List<Station> Stations { get; set; }
    private double CurrentTime { get; set; } = 0;
    private int MachinePerStations;
    private Random Random { get; set; } = new Random();
    public Factory(List<Order> orders, List<Station> stations)
    {
        Orders = CloneListOrder(orders);
        Stations = stations;
    }
    public Factory(List<Order> orders, int machinePerStations)
    {
        Orders = CloneListOrder(orders);
        Stations = new List<Station>();
        MachinePerStations = machinePerStations;
        for (int i = 0; i < orders[0].Steps.Count; i++)
        {
            Stations.Add(new Station(i, machinePerStations));
        }
        FactoryProcess();
    }

    public void MarkProcess()
    {
        if (!AnyAvailableMachine(Stations))
            return;

        Machine avaiableMachine;
        foreach (Order order in Orders)
        {
            if (!order.CanBeProcess())
                continue;

            avaiableMachine = Stations[order.CurrentStep + 1].GetAvaiableMachine(order);
            if (avaiableMachine == null)
                continue;

            order.MarkProcess(ref avaiableMachine, CurrentTime);
        }
    }

    public void Processing()
    {
        double smallestRemainingTime = Stations.SelectMany(station => station.Machines)
            .Where(machine => machine.Status.Equals(MachineStatus.PROCESSING))
            .Select(machine => machine.RemainingTime).Min();

        if (smallestRemainingTime < 0)
            return;

        CurrentTime += smallestRemainingTime;
        foreach (Station station in Stations)
        {
            foreach (Machine machine in station.Machines)
            {
                if (machine.Status.Equals(MachineStatus.NONPROCESSING)) //SKIP THOSE NONPROCESSING MACHINE
                    continue;

                if (machine.RemainingTime == smallestRemainingTime)
                {
                    Orders.First(order => order.Id == machine.OrderId).MarkProcessComplete(CurrentTime);
                    machine.MarkComplete();
                }
                else if (machine.RemainingTime > smallestRemainingTime)
                    machine.RemainingTime -= smallestRemainingTime;
            }
        }
    }

    public void FactoryProcess()
    {
        CurrentTime = 0;
        
        while (!AllOrdersComplete())
        {
            MarkProcess();
            Processing();
        }
    }

    public bool AllOrdersComplete()
    {
        return Orders.All(order => order.Status.Equals(OrderStatus.COMPLETE));
    }

    public double CalculateOverdueTime()
    {
        double overdueTime = 0;
        Orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                overdueTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
        });
        return overdueTime;
    }
    /// <summary>
    /// Create a new list of orders with slightly difference from the original order's list
    /// </summary>
    /// <param name="orders">A list of orders need to be create a neighbor list</param>
    /// <returns>A new list of orders which is slightly difference</returns>
    public List<Order> GenerateNeighbor(List<Order> orders)
    {
        List<Order> neighborList = CloneListOrder(orders);
        int index1 = Random.Next(0, neighborList.Count);
        int index2 = Random.Next(0, neighborList.Count);
        Debug.WriteLine($"SWAP: {index1} <-> {index2}");
        Order temp = neighborList[index1];
        neighborList[index1] = neighborList[index2];
        neighborList[index2] = temp;
        return neighborList;
    }

    /// <summary>
    /// This will create a clone list of Order 
    /// </summary>
    /// <param name="orders">The list of orders need to be clone</param>
    /// <returns>The new List of orders</returns>
    public List<Order> CloneListOrder(List<Order> orders)
    {
        List<Order> result = new List<Order>();
        orders.ForEach(order => result.Add(Order.CloneOrder(order)));
        return result;
    }

    /// <summary>
    /// Check if any machine in station's list is/are avaiable
    /// </summary>
    /// <param name="stations">List of station to check</param>
    /// <returns>True if there is one or many machine(s) avaialbe</returns>
    private static bool AnyAvailableMachine(List<Station> stations)
    {
        return stations.Any(station => station.AreAnyMachineAvaiable());
    }

    /// <summary>
    /// Check if any machine in station is/are available
    /// </summary>
    /// <param name="station">Station to check</param>
    /// <returns>True if there is one or many machine(s) available</returns>
    private static bool AnyAvailableMachine(Station station)
    {
        return station.AreAnyMachineAvaiable();
    }
}