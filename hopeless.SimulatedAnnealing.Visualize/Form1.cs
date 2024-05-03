using LiveCharts.Defaults;
using OxyPlot;
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
            this.Shown += ThisForm_Show;
            List<DataPoint> tempPoints = new List<DataPoint>();
            List<Order> firstOrders = ProcessFirstList(numberOfMachines, filePath, _initTem, _coolingRate);
            List<Order> bestOrders = Process(numberOfMachines, filePath, _initTem, _coolingRate, out tempPoints);
            Extenstions.WriteExcelWithEPPlus(@"C:\Data\OneDrive - a151620\Data\testExcel.xlsx", numberOfMachines, firstOrders, bestOrders);
            double time = SimulatedAnnealingAlgV2.CalculateOverdueTime(bestOrders);
            List<OrderHasColor> orderHasColors = InitializeGanttChart(bestOrders);
            //Debug.WriteLine("Delta T: " + time.ToString("F3"));
            //Debug.WriteLine("\n");
            //bestOrders.ForEach(o => Debug.Write($"{o.Id} -> "));
            //new OrderColor(orderHasColors).Show();

            //new FirstProcess(firstOrders, orderHasColors, SimulatedAnnealingAlgV2.CalculateOverdueTime(firstOrders)).Show();

            new ObjectiveFunctionChart(tempPoints).Show();
        }

        private void ThisForm_Show(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
        }
        public List<Order> Process(int numberOfMachines, string filePath, double initialTemperature, double coolingRate, out List<DataPoint> tempPoints) {

            tempPoints = new List<DataPoint>();
            List<Order> orders = Extenstions.ReadExcelV2(filePath);
            SimulatedAnnealingAlgV2.Init(orders, initialTemperature, coolingRate, numberOfMachines);
            return SimulatedAnnealingAlgV2.PerformSimulatedAnnealingAlgorithmV3(out tempPoints);
            //return sa.TheProcesser(orders);
        }
        public List<Order> ProcessFirstList(int numberOfMachines, string filePath, double initialTemperature, double coolingRate)
        {
            List<Order> orders = Extenstions.ReadExcelV2(filePath);
            SimulatedAnnealingAlgV2.Init(orders, initialTemperature, coolingRate, numberOfMachines);
            return SimulatedAnnealingAlgV2.Process();
        }
        public double GetDeltaT(List<Order> orders, int numberOfMachines, double initialTemperature, double coolingRate) {
            SimulatedAnnealingAlg.MachinesPerStation = numberOfMachines;
            return SimulatedAnnealingAlg.CalculateDelayedTimeV2(orders);
        }
        private List<OrderHasColor> InitializeGanttChart(List<Order> orders)
        {
            double biggestTime = orders.OrderByDescending(order => order.CompleteTime).FirstOrDefault().CompleteTime;
            // Set up the Gantt chart area
            GanttChart chart = new GanttChart(1, biggestTime, SimulatedAnnealingAlgV2.CalculateOverdueTime(orders));
            chart.Dock = DockStyle.Fill;
            Controls.Add(chart);
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
            return orderColors;
        }
    }

}