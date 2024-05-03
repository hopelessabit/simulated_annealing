using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows.Forms;

namespace hopeless.SimulatedAnnealing.Visualize
{
    public partial class ObjectiveFunctionChart : Form
    {
        private List<DataPoint> TempPoints {  get; set; }
        public ObjectiveFunctionChart(List<DataPoint> tempPoints)
        {
            InitializeComponent();
            TempPoints = tempPoints;
            var points = new List<DataPoint>();

            foreach (DataPoint dp in TempPoints)
            {
                points.Add(dp);
            }
            // Create a PlotModel
            var plotModel = new PlotModel { Title = "My Line Chart" };

            // Add a LineSeries
            var lineSeries = new LineSeries();
            lineSeries.Points.AddRange(points);
            plotModel.Series.Add(lineSeries);

            // Add and configure X-axis
            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Loop count" };
            plotModel.Axes.Add(xAxis);

            // Add and configure Y-axis
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Total Tardiness (100s)" };
            plotModel.Axes.Add(yAxis);

            // Set the PlotModel to the PlotView
            plotView1.Model = plotModel;

        }

        private void InitData()
        {
        }
    }
}
