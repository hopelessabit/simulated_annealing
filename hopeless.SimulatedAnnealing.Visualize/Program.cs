using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hopeless.SimulatedAnnealing.Visualize
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());

            //Extenstions.WriteExcelWithEPPlus(@"C:\Data\OneDrive - a151620\Data\testExcel.xlsx", 5);
            //Application.Run(new ObjectiveFunctionChart());
        }

    }
}
