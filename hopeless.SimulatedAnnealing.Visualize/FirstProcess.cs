using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hopeless.SimulatedAnnealing.Visualize
{
    public partial class FirstProcess : Form
    {
        private Random Random { get; set; } = new Random();
        private double TotalTime { get; set; }
        public FirstProcess(List<Order> orders, List<OrderHasColor> orderHasColors, double totalTime)
        {
            InitializeComponent();
            TotalTime = totalTime;
            InitializeGanttChart(orders, orderHasColors);
        }
        private void InitializeGanttChart(List<Order> orders, List<OrderHasColor> orderHasColors)
        {
            double biggestTime = orders.OrderByDescending(order => order.CompleteTime).FirstOrDefault().CompleteTime;
            // Set up the Gantt chart area
            GanttChart chart = new GanttChart(2, biggestTime, SimulatedAnnealingAlgV2.CalculateOverdueTime(orders));
            chart.Dock = DockStyle.Fill;
            Controls.Add(chart);
            // Add tasks to the Gantt chart
            for (int j = 0; j < orders.Count; j++)
            {
                if (orders[j].StartTime == 0)
                    chart.AddTask(orderHasColors[j].Color, orders[j].Steps[0], 0, j, 0);
            }
        }
    }
}
