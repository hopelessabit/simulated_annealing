using System;
using System.Windows.Forms;

namespace hopeless.SimulatedAnnealing.Visualize
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set properties for the OpenFileDialog
            openFileDialog.Title = "Select an Excel File";
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            // Show the dialog and capture the result
            DialogResult result = openFileDialog.ShowDialog();

            // Check if a file was selected
            if (result == DialogResult.OK)
            {
                // Get the selected file path
                string selectedFilePath = openFileDialog.FileName;

                // Do something with the selected file path, such as displaying it in a TextBox
                tbSelectFIle.Text = selectedFilePath;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(int.Parse(tbNumberOfMachine.Text), tbSelectFIle.Text, double.Parse(txbInitTemp.Text), double.Parse(txbCoolingRate.Text));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
