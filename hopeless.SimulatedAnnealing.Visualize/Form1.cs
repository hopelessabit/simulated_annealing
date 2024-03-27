using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
namespace hopeless.SimulatedAnnealing.Visualize
{
    public partial class Form1 : Form
    {
        public Random random = new Random(255);
        private int numberOfMachines { get; }
        private string filePath { get; }
        private double _initTem, _coolingRate;
        public Form1(int numberOfMachines, string filePath, double initTem, double coolingRate)
        {
            this.numberOfMachines = numberOfMachines;
            this.filePath = filePath;
            _initTem = initTem;
            _coolingRate = coolingRate;
            InitializeComponent();
            List<Order> bestOrders = Process(numberOfMachines, filePath, _initTem, _coolingRate);

            double time = SimulatedAnnealingAlgV2.CalculateOverdueTime(bestOrders);
            Debug.WriteLine("Delta T: "+time.ToString("F3"));
            InitializeOrderHasColorChart(bestOrders);
        }
        public List<Order> Process(int numberOfMachines, string filePath, double initialTemperature, double coolingRate) {

            List<Order> orders = Extenstions.ReadExcelV2(filePath);
            //List<Order> orders = new List<Order>
            //    {
            //        new Order(1, new List<Step>() { new Step(9, 1), new Step(1, 2), new Step(0, 3), new Step(3, 4),new Step(0, 3), new Step(3, 4)}, 7),
            //        new Order(2, new List<Step>() { new Step(0, 1), new Step(6, 2), new Step(11, 3), new Step(4, 4),new Step(6, 3), new Step(4, 4)}, 13),
            //        new Order(3, new List<Step>() { new Step(7, 1), new Step(4, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 17),
            //        new Order(4, new List<Step>() { new Step(5, 1), new Step(2, 2), new Step(0, 3), new Step(3, 4),new Step(0, 3), new Step(3, 4)}, 10),
            //        new Order(5, new List<Step>() { new Step(0, 1), new Step(2, 2), new Step(12, 3), new Step(4, 4),new Step(12, 3), new Step(4, 4)}, 14),
            //        new Order(6, new List<Step>() { new Step(4, 1), new Step(8, 2), new Step(3, 3), new Step(0, 4),new Step(3, 3), new Step(0, 4)}, 10),
            //        new Order(0, new List<Step>() { new Step(10, 1), new Step(7, 2), new Step(4, 3), new Step(5, 4), new Step(4, 3), new Step(5, 4)}, 10),
            //    };
            //SimulatedAnnealingAlg sa = new SimulatedAnnealingAlg(orders, initialTemperature, coolingRate, numberOfMachines);
            //return sa.FindBestSolution();

            SimulatedAnnealingAlgV2.Init(orders, initialTemperature, 0.003, 5);
            return SimulatedAnnealingAlgV2.PerformSimulatedAnnealingAlgorithmV2();
            //return sa.TheProcesser(orders);
        }
        public double GetDeltaT(List<Order> orders, int numberOfMachines, double initialTemperature, double coolingRate) {
            SimulatedAnnealingAlg.MachinesPerStation = numberOfMachines;
            return SimulatedAnnealingAlg.CalculateDelayedTimeV2(orders);
        }

        private void InitializeOrderHasColorChart(List<Order> orders)
        {
            
            double biggestTime = orders.OrderByDescending(order => order.CompleteTime).FirstOrDefault().CompleteTime;
            // Set up the Gantt chart area
            OrderHasColorChart chart = new OrderHasColorChart(biggestTime);
            chart.Dock = DockStyle.Fill;
            Controls.Add(chart);
            Color.FromArgb(1,1,1);
            // Add tasks to the Gantt chart
            List<Color> colors = new List<Color>();
            List<OrderHasColor> orderColors = new List<OrderHasColor>();
            for (int i = 0; i < orders.Count;  i++)
            {
                colors.Add(Color.FromArgb(random.Next(0, 200), random.Next(0, 200), random.Next(0, 200)));
            }
            for (int j = 0; j < orders.Count; j++)
            {
                for (int i = 0; i < orders[j].Steps.Count; i++)
                {
                    chart.AddTask(colors[j], orders[j].Steps[i], i, j, i); 
                }
                orderColors.Add(new OrderHasColor(orders[j].Name, colors[j]));
            }

            // Add more tasks as needed
        }
    }

}