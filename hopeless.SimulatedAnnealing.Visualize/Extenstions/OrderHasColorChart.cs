using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

public class OrderHasColorChart : UserControl
{
    private static int A = 0;
    private readonly Pen taskPen = new Pen(Color.Black);
    private readonly Brush taskBrush = new SolidBrush(Color.LightBlue);
    private readonly Font taskFont = new Font("Arial", 8);
    private readonly int taskHeight = 20;
    private List<OrderHasColor> OrderHasColors {  get; set; }
    private double TotalTime { get; set; }

    public OrderHasColorChart(List<OrderHasColor> orderHasColors)
    {
        OrderHasColors = orderHasColors;
        foreach (OrderHasColor color in orderHasColors)
        {
            AddTask(color);
        }
    }

    private struct Task
    {
        public Color Color;
        public string Name;
    }

    private readonly System.Collections.Generic.List<Task> tasks = new System.Collections.Generic.List<Task>();

    public void AddTask(OrderHasColor order)
    {
        tasks.Add(new Task { Color = order.Color, Name = order.OrderName }) ;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;

        //Draw tasks
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            int startY = 450 / 10 * (i % 10);
            int height = taskHeight;
            float startX = 800 / 4 * (i / 10);
            float width = 20;

            g.DrawString($"{tasks[i].Name}", taskFont, Brushes.Black, startX + 22, startY);
            g.FillRectangle(new SolidBrush(task.Color), startX, startY, width, height);

            //Debug.Write($"{task.OrderId},{task.StepId}:{task.EndTime.ToString("F3")}\t\t -\t\t {task.StartTime.ToString("F3")}\t\t");
            //Debug.WriteLine((float)((task.EndTime - task.StartTime) * 1400 / TotalTime));
        }
    }
}