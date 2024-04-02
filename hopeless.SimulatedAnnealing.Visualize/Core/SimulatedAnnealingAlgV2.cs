using Microsoft.VisualBasic;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class SimulatedAnnealingAlgV2
{
    public static List<Order> MainOrderList;
    public static double InitialTemperature;
    public static double CoolingRate;
    public static int MachinesPerStation;
    public static Random Random = new Random();

    public static void Init(List<Order> orderList, double initialTemp, double coolingRate, int machinesPerStation)
    {
        MainOrderList = orderList;
        InitialTemperature = initialTemp;
        CoolingRate = coolingRate;
        MachinesPerStation = machinesPerStation;
    }

    public static List<Order> PerformSimulatedAnnealingAlgorithm()
    {
        List<Order> currentSolution = MainOrderList;
        List<Order> bestSolution = CloneListOrder(currentSolution);

        double temp = InitialTemperature;

        while (temp > 1)
        {
            List<Order> newSolution = GenerateNeighbor(currentSolution);
            Factory currentSolutionFactory = new Factory(currentSolution, 4);
            Factory newSolutionFactory = new Factory(newSolution, 4);
            double currentEnergy = currentSolutionFactory.CalculateOverdueTime();
            double neighborEnergy = newSolutionFactory.CalculateOverdueTime();


            Debug.WriteLine("\nCurent order: ");
            currentSolutionFactory.Orders.ForEach(order => Debug.Write(order.Id + " "));
            Debug.WriteLine("\nNeighbor order: ");
            newSolutionFactory.Orders.ForEach(order => Debug.Write(order.Id + " "));
            Debug.WriteLine("");
            double acceptanceProbability = AcceptanceProbability(currentEnergy, neighborEnergy, temp);
            Debug.WriteLine($"{currentEnergy} - {neighborEnergy}");
            if (acceptanceProbability > Random.NextDouble() )
            {
                Debug.WriteLine($"\t\taccept \t cur: {currentEnergy.ToString("F2")} - nei: {neighborEnergy.ToString("F2")} : {currentEnergy > neighborEnergy} ");
                currentSolution = CloneListOrder(newSolution);
            }
            currentSolutionFactory = new Factory(currentSolution, 4);
            newSolutionFactory = new Factory(newSolution, 4);
            if (currentSolutionFactory.CalculateOverdueTime() < newSolutionFactory.CalculateOverdueTime())
            {
                bestSolution = CloneListOrder(currentSolution);
            }

            temp *= 1 - CoolingRate;
        }

        return bestSolution;
    }
    public static List<Order> PerformSimulatedAnnealingAlgorithmV2()
    {
        double temp = InitialTemperature;
        Factory currentSolutionFactory = new Factory(MainOrderList, MachinesPerStation);
        double currentEnergy = currentSolutionFactory.CalculateOverdueTime();
        for (int i = 0; i < 120; i++)
        { 
            Factory neighborSolutionFactory = new Factory(GenerateNeighbor(currentSolutionFactory.Orders), MachinesPerStation);
            double neighborEnergy = neighborSolutionFactory.CalculateOverdueTime();

            double acceptanceProbability = AcceptanceProbability(currentEnergy, neighborEnergy, temp * Math.Pow(CoolingRate, i));
            double r = Random.NextDouble();
            Debug.Write($"current: {currentEnergy.ToString("F3")} - neighbor: {neighborEnergy.ToString("F3")} \t\t\t|{r.ToString("F3")} < {acceptanceProbability.ToString("F3")} : {r < acceptanceProbability} |\t\t\t");
            if (r < acceptanceProbability)
            {
                currentSolutionFactory = neighborSolutionFactory;
                currentEnergy = neighborEnergy;
            }
            Debug.WriteLine($"current: {currentEnergy.ToString("F3")} - neighbor: {neighborEnergy.ToString("F3")}");

            temp *= 1 - CoolingRate;
        }

        Debug.WriteLine("\n");
        currentSolutionFactory.Orders.ForEach(o => Debug.Write($"{o.Id} -> "));
        return currentSolutionFactory.Orders;
    }
    public static List<Order> PerformSimulatedAnnealingAlgorithmV3(out List<DataPoint> tempPoints)
    { 
        double temp = InitialTemperature;
        tempPoints = new List<DataPoint>();
        Factory currentSolutionFactory = new Factory(MainOrderList, MachinesPerStation);
        double currentEnergy = currentSolutionFactory.CalculateOverdueTime();
        int count = 0;
        while (temp > 1)
        {
            Factory neighborSolutionFactory = new Factory(GenerateNeighbor(currentSolutionFactory.Orders), MachinesPerStation);
            double neighborEnergy = neighborSolutionFactory.CalculateOverdueTime();

            double acceptanceProbability = AcceptanceProbability(currentEnergy, neighborEnergy, temp);
            double r = Random.NextDouble();
            Debug.Write($"current: {currentEnergy.ToString("F3")} - neighbor: {neighborEnergy.ToString("F3")} \t\t\t|{r.ToString("F3")} < {acceptanceProbability.ToString("F3")} : {r < acceptanceProbability} |\t\t\t");
            if (r < acceptanceProbability)
            {
                Debug.WriteLine("==== " + (r < acceptanceProbability).ToString());
                currentSolutionFactory = neighborSolutionFactory;
                currentEnergy = neighborEnergy;
            }
            if (++count % 50 == 0 || count == 1)   
            {
                tempPoints.Add(new DataPoint(count, Math.Round(currentEnergy / 100, 2)));
            }
            temp *= (1 - CoolingRate);
        }
        tempPoints.Add(new DataPoint(count, Math.Round(currentEnergy / 100, 2)));

        Debug.WriteLine("\n");
        currentSolutionFactory.Orders.ForEach(o => Debug.Write($"{o.Id} -> "));
        return currentSolutionFactory.Orders;
    }
    public static List<Order> Process()
    {
        Factory factory = new Factory(MainOrderList, MachinesPerStation);
        factory.FactoryProcess();
        return factory.Orders;
    }

    /// <summary>
    /// Calculate the overdue time of an order
    /// </summary>
    /// <param name="order">The order must be complete</param>
    /// <returns>Return 0 if (process time '<' expected duration)</returns>
    /// <returns></returns>

    public static double CalculateOverdueTime(List<Order> orders)
    {
        double overdueTime = 0;
        orders.ForEach(order =>
        {
            if (order.CompleteTime - order.StartTime - order.ExpectedDuration > 0)
                overdueTime += order.CompleteTime - order.StartTime - order.ExpectedDuration;
        });
        return overdueTime;
    }

    /// <summary>
    /// Init values for SimulatedAnnealingAlgV2 static class
    /// </summary>
    /// <param name="initTemp">Initial temperature</param>
    /// <param name="coolingRate">Cooling rate</param>
    /// <param name="orders">List of orders</param>
    public static void Init(double initTemp, double coolingRate, List<Order> orders)
    {
        MainOrderList = orders;
        InitialTemperature = initTemp;
        CoolingRate = coolingRate;
    }

    /// <summary>
    /// Calculate the probability of the case being accept
    /// </summary>
    /// <param name="currentEnergy">The energy of current order's list</param>
    /// <param name="neighborEnergy">The energy of neighbor order's list</param>
    /// <param name="temperature">The current temperature</param>
    /// <returns></returns>
    public static double AcceptanceProbability(double currentEnergy, double neighborEnergy, double temperature)
    {
        if (neighborEnergy < currentEnergy) //chac chan se lay nho hon
        {
            return 1.0;
        }
        var a =  Math.Exp((currentEnergy - neighborEnergy) / (temperature));
        return Math.Exp((currentEnergy - neighborEnergy) / (temperature));
    }

    /// <summary>
    /// Create a new list of orders with slightly difference from the original order's list
    /// </summary>
    /// <param name="orders">A list of orders need to be create a neighbor list</param>
    /// <returns>A new list of orders which is slightly difference</returns>
    public static List<Order> GenerateNeighbor(List<Order> orders)
    {
        List<Order> neighborList = CloneListOrder(orders);
        int index1 = Random.Next(0, neighborList.Count);
        int index2 = Random.Next(0, neighborList.Count);
        Debug.WriteLine($"\t\t -------------SWAP: {neighborList[index1].Id} <-> {neighborList[index2].Id}");
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
    public static List<Order> CloneListOrder(List<Order> orders)
    {
        List<Order> result = new List<Order>();
        orders.ForEach(order => result.Add(order.Clone()));
        return result;
    }

}
public class TempPoint
{
    public double Temp {  get; set; }
    public int LoopCount {  get; set; }
    public TempPoint(double temp, int loopNumber)
    {
        Temp = temp;
        LoopCount = loopNumber;
    }
}