using System.Drawing;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using static System.Windows.Forms.AxHost;

public class GanttChart : UserControl
{
    private static int A = 0;
    private readonly Pen taskPen = new Pen(Color.Black);
    private readonly Brush taskBrush = new SolidBrush(Color.LightBlue);
    private readonly Font taskFont = new Font("Arial", 5);
    private readonly int taskHeight = 20;
    private double TotalTime { get; set; } = 0;
    private double OverdueTime { get; set; } = 0;
    private int State { get; set; } //1: draw full process   |    2: draw first process

    public GanttChart(int state,double totalTime, double overdueTime)
    {
        State = state;
        TotalTime = totalTime;
        OverdueTime = overdueTime;
    }

    public GanttChart() { }
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
        tasks.Add(new Task
        {
            Color = color,
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

        for (int i = 0; i < 6; i++)
        {
            int startY = 150 * i + 18;
            int height = 120;

            float startX = 200;
            float width = 1400;
            string[] stationName = new string[6] { "Spinning machine", "Prod68 1", "Prod68 2", "Prod59", "Sizing, ironing", "Packaging" };
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 209, 209, 224)), startX, startY, width, height);
            //g.DrawString($"Station no.{i}", taskFont, Brushes.Black, 75, 75 + 150 * i);
            g.DrawString($"{stationName[i]}", taskFont, Brushes.Black, 75, 75 + 150 * i);
        }
        if (State == 1) DrawFullProcessGanttChart(g);
        else DrawFirstProcessGanttChart(g);
    }

    public void DrawFullProcessGanttChart(Graphics g)
    {
        int month = 5;
        int day = 0;
        for (int i = 0; i < Math.Ceiling(TotalTime / 57600); i++)
        {
            day++;
            int startY = 18;
            int height = 150 * 6;

            float startX = 200 + (float)(1400 / Math.Ceiling(TotalTime / 57600) * i);
            float width = 3;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255, 255)), startX, startY, width, height);
            if (month == 5 && day > 31) {
                day = 1;
                month++;
            }
            else if (month == 6 && day > 30){
                day =1;
                month++;
            }
            else if (month == 7 && day > 31) {
                day =1;
                month++;
            }
            g.DrawString($"{day.ToString("D2")}/{month.ToString("00")}", taskFont, Brushes.Black, (float)(190 + i * (1400 / Math.Ceiling(TotalTime / 57600))), 155 + 150 * 5);
        }
        if (month == 7 && day > 31)
        {
            day = 1;
            month++;
        }
        g.DrawString($"{(day + 1).ToString("D2")}/{month.ToString("00")}", taskFont, Brushes.Black, 1590, 155 + 150 * 5);


        //Draw tasks
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            int startY = (task.StationId) * (130 + 20) + (task.MachineId) * (20 + 3) + 20;
            int height = taskHeight;


            float startX = (float)((task.StartTime / TotalTime * 1400) + 200);
            float width = (float)((task.EndTime - task.StartTime) * 1400 / TotalTime);
            if (width < 1 && ((task.EndTime - task.StartTime) != 0)) width = 1;
            g.FillRectangle(new SolidBrush(task.Color), startX, startY, width, height);
        }
        g.DrawString($"Total Time(s): {TotalTime.ToString("F3")}", taskFont, Brushes.Black, 0, 0);
        g.DrawString($"Total Tardiness(s): {OverdueTime.ToString("F3")}", taskFont, Brushes.Black, 0, 20);
    }
    public void DrawFirstProcessGanttChart(Graphics g)
    {
        int month = 5;
        int day = 0;
        for (int i = 0; i < Math.Ceiling(TotalTime / 57600); i++)
        {
            day++;
            int startY = 18;
            int height = 150 * 6;

            float startX = 200 + (float)(1400 / Math.Ceiling(TotalTime / 57600) * i);
            float width = 3;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255, 255)), startX, startY, width, height);
            if (month == 5 && day > 31)
            {
                day = 1;
                month++;
            }
            else if (month == 6 && day > 30)
            {
                day = 1;
                month++;
            }
            else if (month == 7 && day > 31)
            {
                day = 1;
                month++;
            }
            g.DrawString($"{day.ToString("D2")}/{month.ToString("00")}", taskFont, Brushes.Black, (float)(190 + i * (1400 / Math.Ceiling(TotalTime / 57600))), 155 + 150 * 5);
        }
        if (month == 7 && day > 31)
        {
            day = 1;
            month++;
        }
        g.DrawString($"{(day + 1).ToString("D2")}/{month.ToString("00")}", taskFont, Brushes.Black, 1590, 155 + 150 * 5);

        //Draw tasks
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            int startY = (task.StationId) * (130 + 20) + (task.MachineId) * (20 + 3) + 20;
            int height = taskHeight;


            float startX = (float)((task.StartTime / TotalTime * 1400) + 200);
            float width = (float)((task.EndTime - task.StartTime) * 1400 / TotalTime);
            if (width < 1 && ((task.EndTime - task.StartTime) != 0)) width = 1;
            g.FillRectangle(new SolidBrush(task.Color), startX, startY, width, height);

        }
        g.DrawString($"Total Time(s): {TotalTime.ToString("F3")}", taskFont, Brushes.Black, 0, 0);
        g.DrawString($"Total Tardiness(s): {OverdueTime.ToString("F3")}", taskFont, Brushes.Black, 0, 20);
    }
}
