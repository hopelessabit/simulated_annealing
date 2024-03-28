using OfficeOpenXml.Style;
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
    public partial class OrderColor : Form
    {
        public List<OrderHasColor> OrderHasColorList;
        public OrderColor(List<OrderHasColor> orders)
        {
            OrderHasColorList = orders;

            InitializeComponent();
            DrawOrderColor();
            this.Shown += ThisForm_Show;
        }

        public void DrawOrderColor()
        {

            // Set up the Gantt chart area
            OrderHasColorChart chart = new OrderHasColorChart(OrderHasColorList);
            chart.Dock = DockStyle.Fill;
            Controls.Add(chart);
        }

        private void ThisForm_Show(object sender, EventArgs e)
        {
            this.Location = new Point(1000, 70);
            this.TopMost = true;
        }
    }
}
