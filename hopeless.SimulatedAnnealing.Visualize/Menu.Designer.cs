namespace hopeless.SimulatedAnnealing.Visualize
{
    partial class Menu
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
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            tbSelectFIle = new System.Windows.Forms.TextBox();
            btnSelectFile = new System.Windows.Forms.Button();
            tbNumberOfMachine = new System.Windows.Forms.TextBox();
            btnProcess = new System.Windows.Forms.Button();
            txbInitTemp = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txbCoolingRate = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 43);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(28, 15);
            label1.TabIndex = 0;
            label1.Text = "File ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 88);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(207, 15);
            label2.TabIndex = 1;
            label2.Text = "Number of machine(s) in each station";
            // 
            // tbSelectFIle
            // 
            tbSelectFIle.Location = new System.Drawing.Point(46, 40);
            tbSelectFIle.Name = "tbSelectFIle";
            tbSelectFIle.Size = new System.Drawing.Size(565, 23);
            tbSelectFIle.TabIndex = 3;
            tbSelectFIle.Text = "C:\\Users\\mical\\Downloads\\B-1.xlsx";
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new System.Drawing.Point(617, 40);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new System.Drawing.Size(75, 23);
            btnSelectFile.TabIndex = 4;
            btnSelectFile.Text = "Select File";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // tbNumberOfMachine
            // 
            tbNumberOfMachine.Location = new System.Drawing.Point(225, 85);
            tbNumberOfMachine.Name = "tbNumberOfMachine";
            tbNumberOfMachine.Size = new System.Drawing.Size(131, 23);
            tbNumberOfMachine.TabIndex = 5;
            tbNumberOfMachine.Text = "5";
            // 
            // btnProcess
            // 
            btnProcess.Location = new System.Drawing.Point(362, 231);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new System.Drawing.Size(75, 23);
            btnProcess.TabIndex = 6;
            btnProcess.Text = "PROCESS";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;
            // 
            // txbInitTemp
            // 
            txbInitTemp.Location = new System.Drawing.Point(225, 119);
            txbInitTemp.Name = "txbInitTemp";
            txbInitTemp.Size = new System.Drawing.Size(131, 23);
            txbInitTemp.TabIndex = 8;
            txbInitTemp.Text = "1000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 122);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(105, 15);
            label3.TabIndex = 7;
            label3.Text = "Initial Temperature";
            label3.Click += label3_Click;
            // 
            // txbCoolingRate
            // 
            txbCoolingRate.Location = new System.Drawing.Point(225, 154);
            txbCoolingRate.Name = "txbCoolingRate";
            txbCoolingRate.Size = new System.Drawing.Size(131, 23);
            txbCoolingRate.TabIndex = 10;
            txbCoolingRate.Text = "0.003";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 157);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(75, 15);
            label4.TabIndex = 9;
            label4.Text = "Cooling Rate";
            // 
            // Menu
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(800, 266);
            Controls.Add(txbCoolingRate);
            Controls.Add(label4);
            Controls.Add(txbInitTemp);
            Controls.Add(label3);
            Controls.Add(btnProcess);
            Controls.Add(tbNumberOfMachine);
            Controls.Add(btnSelectFile);
            Controls.Add(tbSelectFIle);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Menu";
            Text = "Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSelectFIle;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox tbNumberOfMachine;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txbInitTemp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbCoolingRate;
        private System.Windows.Forms.Label label4;
    }
}