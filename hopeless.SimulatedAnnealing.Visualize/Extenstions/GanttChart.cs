﻿using System.Drawing;
using System.Windows.Forms;
using System;
using System.Diagnostics;

public class GanttChart : UserControl
{
    private static int A = 0;
    private readonly Pen taskPen = new Pen(Color.Black);
    private readonly Brush taskBrush = new SolidBrush(Color.LightBlue);
    private readonly Font taskFont = new Font("Arial", 8);
    private readonly int taskHeight = 20;
    private double TotalTime { get; set; }
    private double OverdueTime { get; set; }

    public GanttChart(double totalTime, double overdueTime)
    {
        TotalTime = totalTime;
        OverdueTime = overdueTime;
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

        for (int i = 0; i < Math.Ceiling(TotalTime / 57600); i++)
        {
            int startY = 18;
            int height = 150 * 6;

            float startX = 400 + (float)(1400 / Math.Ceiling(TotalTime / 57600) * i);
            float width = 3;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255, 255)), startX, startY, width, height);
            g.DrawString($"{(3 + i).ToString("D2")}/05/2023", taskFont, Brushes.Black, (float)(170 + i * (1400 / Math.Ceiling(TotalTime / 57600))), 155 + 150 * 5);
        }
        g.DrawString($"{3 + (Math.Ceiling(TotalTime / 57600) + 1)}/05/2023", taskFont, Brushes.Black, 1550, 155 + 150 * 5);


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

            Debug.Write($"{task.OrderId},{task.StepId}:{task.EndTime.ToString("F3")}\t\t -\t\t {task.StartTime.ToString("F3")}\t\t");
            Debug.WriteLine((float)((task.EndTime - task.StartTime) * 1400 / TotalTime));
        }
        g.DrawString($"Total Time(s): {TotalTime.ToString("F3")}", taskFont, Brushes.Black, 0, 0);
        g.DrawString($"Overdue Time(s): {OverdueTime.ToString("F3")}", taskFont, Brushes.Black, 0, 20);
    }
}