using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    static Random random = new Random();
    static int CurrentTime = 0;
    public static void Main()
    {
        List<Step> a = new List<Step>() { new Step(2), new Step(0), new Step(4), new Step(5) };

        List<Order> orders = new List<Order>
        {
            new Order("Order 1", new List<Step>() { new Step(2), new Step(0), new Step(4), new Step(5) }, 10),
            new Order("Order 2", new List<Step>() { new Step(5), new Step(1), new Step(0), new Step(3) }, 7),
            new Order("Order 3", new List<Step>() { new Step(0), new Step(2), new Step(6), new Step(4) }, 13),
            new Order("Order 4", new List<Step>() { new Step(7), new Step(4), new Step(3), new Step(0) }, 17),
            new Order("Order 5", new List<Step>() { new Step(5), new Step(2), new Step(0), new Step(3) }, 10),
            new Order("Order 6", new List<Step>() { new Step(0), new Step(2), new Step(12), new Step(4) }, 14),
            new Order("Order 7", new List<Step>() { new Step(4), new Step(8), new Step(3), new Step(0) }, 10)
        };

        List<Station> stations = new List<Station>
        {
            new Station("Station 1", 4),
            new Station("Station 2", 4),
            new Station("Station 3", 4)
        };

        ShuffleOrders(orders);


    }
    static int CheckAvaiableMachine(int step, List<Station> stations)
    {
        for (int i = 0; i < stations.Count; i++)
        {
            if (stations[i].Machines[step].Status.Equals(MachineStatus.UNPROCESSING))
                return i;
        }
        return -1; //-1 mean all the machines to process the step is not avaiable
    }
    static List<int> GetAvaiableMachine(int step, List<Station> stations)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < stations.Count; i++)
        {
            if (!stations[i].Machines[step].Status.Equals(MachineStatus.UNPROCESSING))
                result.Add(i);
        }
        return result;
    }
    static void PerformFirstProcessing(Order order, List<Station> stations, int avaiableMachineIndex)
    {
        stations[avaiableMachineIndex].Machines[0].MarkProcessing(0, order.Steps[0].RequireTime);
        order.Steps[0].MarkProcessing(avaiableMachineIndex);
    }
    /*
     * return true if last step.
     */
    static void SetOrderProcessing(int orderId, Order order, int stationId, List<Station> stations)
    {
        order.NextStep();
        order.MarkStepProcessing(stationId);
        stations[stationId].MarkProcessing(orderId, order);
    }
    static void SetProcessingFirstTime(int orderId, Order order, int stationId, List<Station> stations)
    {
        order.MarkStepProcessingFirstTime(stationId);
        order.Steps[0].MarkProcessing(stationId);
        stations[stationId].MarkProcessing(orderId, order);
    }
    static void SetProcessCompleted(Order order, int stationId, List<Station> station)
    {
        order.MarkComplete();
        station[stationId].MarkComplete(order);
    }
    static void ShuffleOrders(List<Order> orders)
    {
        // Use Fisher-Yates shuffle algorithm to shuffle the orders
        int n = orders.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Order temp = orders[k];
            orders[k] = orders[n];
            orders[n] = temp;
        }
    }

}