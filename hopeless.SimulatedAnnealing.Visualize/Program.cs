using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hopeless.SimulatedAnnealing.Visualize
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Menu());
            Application.Run(new Form1(5, @"C:\Users\mical\Downloads\B.xlsx", 1000, 0.003));


            //List<Order> orders = new List<Order>
            //    {
            //        new Order(1, new List<Step>() { new Step(9, 1), new Step(1, 2), new Step(0, 3), new Step(3, 4),new Step(0, 3), new Step(3, 4)}, 7),
            //        new Order(4, new List<Step>() { new Step(5, 1), new Step(2, 2), new Step(0, 3), new Step(3, 4),new Step(0, 3), new Step(3, 4)}, 10),
            //        new Order(3, new List<Step>() { new Step(7, 1), new Step(4, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 17),
            //        new Order(2, new List<Step>() { new Step(0, 1), new Step(6, 2), new Step(11, 3), new Step(4, 4),new Step(6, 3), new Step(4, 4)}, 13),
            //        new Order(0, new List<Step>() { new Step(10, 1), new Step(7, 2), new Step(4, 3), new Step(5, 4), new Step(4, 3), new Step(5, 4)}, 10),
            //        new Order(5, new List<Step>() { new Step(0, 1), new Step(2, 2), new Step(12, 3), new Step(4, 4),new Step(12, 3), new Step(4, 4)}, 14),
            //        new Order(6, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 10),
            //        new Order(7, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3s), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 17),
            //        new Order(8, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 1),
            //        new Order(9, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 15),
            //        new Order(10, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 2),
            //        new Order(11, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 10),
            //    };
            ////Factory factory = new Factory(orders, 2);
            ////Debug.WriteLine(factory.CalculateOverdueTime());

            //SimulatedAnnealingAlgV2.Init(orders, 1000, 0.003);
            //List<Order> result = SimulatedAnnealingAlgV2.PerformSimulatedAnnealingAlgorithmV2();
            //result.ForEach(order => Debug.WriteLine(order.ToString()));
        }

    }
}
