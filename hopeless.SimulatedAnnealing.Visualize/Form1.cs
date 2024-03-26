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

            double time = GetDeltaT(bestOrders, numberOfMachines, _initTem, _coolingRate);
            Debug.WriteLine("Delta T: "+time.ToString("F3"));
            InitializeGanttChart(bestOrders);
        }
        public List<Order> Process(int numberOfMachines, string filePath, double initialTemperature, double coolingRate) { 

            List<Order> orders = Extenstions.ReadExcelV2(filePath);
            SimulatedAnnealingAlg sa = new SimulatedAnnealingAlg(orders, initialTemperature, coolingRate, numberOfMachines);
            return sa.FindBestSolution();
            //return sa.TheProcesser(orders);
        }
        public double GetDeltaT(List<Order> orders, int numberOfMachines, double initialTemperature, double coolingRate) {
            SimulatedAnnealingAlg.MachinesPerStation = numberOfMachines;
            return SimulatedAnnealingAlg.CalculateDelayedTimeV2(orders);
        }

        private void InitializeGanttChart(List<Order> orders)
        {
            
            double biggestTime = orders.OrderByDescending(order => order.CompleteTime).FirstOrDefault().CompleteTime;
            // Set up the Gantt chart area
            GanttChart chart = new GanttChart(biggestTime);
            chart.Dock = DockStyle.Fill;
            Controls.Add(chart);
            Color.FromArgb(1,1,1);
            // Add tasks to the Gantt chart
            List<Color> colors = new List<Color>();
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
            }

            // Add more tasks as needed
        }
    }

    public class GanttChart : UserControl
    {
        private static int A = 0;
        private readonly Pen taskPen = new Pen(Color.Black);
        private readonly Brush taskBrush = new SolidBrush(Color.LightBlue);
        private readonly Font taskFont = new Font("Arial", 8);
        private readonly int taskHeight = 20;
        private double TotalTime { get; set; }

        public GanttChart(double totalTime)
        {
            TotalTime = totalTime;
        }

        private struct Task
        {
            public Color Color;
            public double StartTime;
            public double EndTime;
            public int StationId;
            public int MachineId;
            public int OrderId;
            public int StepId;
        }

        private readonly System.Collections.Generic.List<Task> tasks = new System.Collections.Generic.List<Task>();

        public void AddTask(string name, Color color, double startTime, double endTime)
        {
            tasks.Add(new Task { Color = color, StartTime = startTime, EndTime = endTime });
            Invalidate();
        }
        public void AddTask(Color color, Step step, int id, int orderId, int stepId)
        {
            tasks.Add(new Task {Color = color, 
                StartTime = step.StartTime, 
                EndTime = step.CompleteTime, 
                StationId = id, 
                MachineId = step.MachineProcess,
                OrderId = orderId,
                StepId = stepId
            });
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255, 255)), 0, 0, 1920, 1080);

            for (int i = 0; i < 6; i++)
            {
                int startY = 150 * i + 18;
                int height = 120;

                float startX = 200;
                float width = 1400;
                string[] stationName = new string[6] { "Spinning machine", "Prod68 1", "Prod68 2", "Prod59", "Sizing, ironing", "Packaging"};
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 209, 209, 224)), startX, startY, width, height);
                //g.DrawString($"Station no.{i}", taskFont, Brushes.Black, 75, 75 + 150 * i);
                g.DrawString($"{stationName[i]}", taskFont, Brushes.Black, 75, 75 + 150 * i);
            }

            for (int i = 0; i < Math.Ceiling(TotalTime / 57600); i++)
            {
                int startY = 18;
                int height = 150 * 6;

                float startX = 400 + (float) (1400 / Math.Ceiling(TotalTime / 57600) * i);
                float width = 3;
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255, 255)), startX, startY, width, height);
                g.DrawString($"0{3 + i}/05/2023", taskFont, Brushes.Black, (float) (170 + i * (1400 / Math.Ceiling(TotalTime / 57600))) , 155 + 150 * 5);
            }
                g.DrawString($"0{3 + (Math.Ceiling(TotalTime / 57600) + 1)}/05/2023", taskFont, Brushes.Black, 1550, 155 + 150 * 5);


            //Draw tasks
            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[i];
                int startY = (task.StationId) * (130 + 20) + (task.MachineId) * (20 + 3) + 20;
                int height = taskHeight;


                float startX = (float)(task.StartTime / TotalTime * 1400) + 200;
                float width = (float)((task.EndTime - task.StartTime) * 1400/ TotalTime);
                if (width < 1 && ((task.EndTime - task.StartTime) != 0)) width = 1;
                g.FillRectangle(new SolidBrush(task.Color), startX, startY, width, height);
                //Debug.WriteLine($"x={startX}\t\t: y={startY}\t\t-\t\twitdh=({task.EndTime} - {task.StartTime})/{TotalTime}*1400 = {width}");
                Debug.Write($"{task.OrderId},{task.StepId}:{task.EndTime.ToString("F3")}\t\t -\t\t {task.StartTime.ToString("F3")}\t\t");
                Debug.WriteLine((float)((task.EndTime - task.StartTime) * 1400 / TotalTime));
                // Draw task name
                //g.DrawString($"{((task.EndTime - task.StartTime) / TotalTime).ToString("0.00")}", taskFont, Brushes.Black, startX, startY + height);
            }
            g.DrawString($"{TotalTime}", taskFont, Brushes.Black, 0, 0);
            g.FillRectangle(new SolidBrush(tasks[0].Color), 200, 11, (float)(TotalTime/TotalTime * 1400), 10);
        }
    }
}