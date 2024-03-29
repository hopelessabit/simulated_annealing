namespace hopeless.SimulatedAnnealing.Visualize
{
    partial class ObjectiveFunctionChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            plotView1 = new OxyPlot.WindowsForms.PlotView();
            SuspendLayout();
            // 
            // plotView1
            // 
            plotView1.Location = new System.Drawing.Point(77, 21);
            plotView1.Name = "plotView1";
            plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            plotView1.Size = new System.Drawing.Size(536, 310);
            plotView1.TabIndex = 0;
            plotView1.Text = "plotView1";
            plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // ObjectiveFunctionChart
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(plotView1);
            Name = "ObjectiveFunctionChart";
            Text = "ObjectiveFunctionChart";
            ResumeLayout(false);
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotView1;
    }
}