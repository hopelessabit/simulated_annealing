using OfficeOpenXml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class Extenstions
{

    public static List<Order> ReadExcel(string filePath)
    {
        List<Order> result = new List<Order>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            // Get the first worksheet (adjust sheet index if needed)
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            // Start reading data from a specific row (replace 1 with your starting row)
            int currentRow = 1;

            while (true)
            {
                //string check = worksheet.Cells[currentRow, 1].Value.ToString();
                // Check if all cells are empty, signifying end of data
                if (worksheet.Cells[currentRow, 1].Value == null)
                {
                    break;
                }
                // Read data from each cell in the current row
                int orderId = int.Parse(worksheet.Cells[currentRow, 1].Value.ToString());
                string orderName = worksheet.Cells[currentRow, 2].Value.ToString();
                int quantity = int.Parse(worksheet.Cells[currentRow, 3].Value.ToString());
                double expectCompleteTime = double.Parse(worksheet.Cells[currentRow, 4].Value.ToString());
                double step1 = double.Parse((worksheet.Cells[currentRow, 5].Value.ToString()));
                double step2 = double.Parse((worksheet.Cells[currentRow, 6].Value.ToString()));
                double step3 = double.Parse((worksheet.Cells[currentRow, 7].Value.ToString()));
                double step4 = double.Parse((worksheet.Cells[currentRow, 8].Value.ToString()));
                double step5 = double.Parse((worksheet.Cells[currentRow, 9].Value.ToString()));
                double step6 = double.Parse((worksheet.Cells[currentRow, 10].Value.ToString()));
                //Console.WriteLine($"{orderId} \t {orderName} \t {quantity} \t {expectCompleteTime} \t {step1} \t {step2} \t {step3} \t {step4} \t {step5} \t {step6}");
                result.Add(new Order(orderId, orderName, new List<Step>() { new Step(step1, 0), new Step(step2, 2), new Step(step3, 3), new Step(step4, 4), new Step(step5, 5), new Step(step6, 6) }, expectCompleteTime));
                currentRow++;
            }
        }
        return result;
    }    
    public static List<Order> ReadExcelV2(string filePath)
    {
        List<Order> result = new List<Order>();
        List<int> orders = new List<int>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            // Get the first worksheet (adjust sheet index if needed)
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            // Start reading data from a specific row (replace 1 with your starting row)
            int currentRow = 1;

            while (true)
            {
                if (worksheet.Cells[currentRow, 1].Value == null)
                {
                    break;
                }
                // Read data from each cell in the current row
                int orderId = int.Parse(worksheet.Cells[currentRow, 14].Value.ToString());
                orders.Add(orderId);
                currentRow++;
            }

            foreach (int id  in orders)
            {
                //string check = worksheet.Cells[currentRow, 1].Value.ToString();
                // Check if all cells are empty, signifying end of data
                if (worksheet.Cells[id, 1].Value == null)
                {
                    break;
                }
                // Read data from each cell in the current row
                int orderId = int.Parse(worksheet.Cells[id, 1].Value.ToString());
                string orderName = worksheet.Cells[id, 2].Value.ToString();
                int quantity = int.Parse(worksheet.Cells[id, 3].Value.ToString());
                double expectCompleteTime = double.Parse(worksheet.Cells[id, 4].Value.ToString());
                double step1 = double.Parse((worksheet.Cells[id, 5].Value.ToString()));
                double step2 = double.Parse((worksheet.Cells[id, 6].Value.ToString()));
                double step3 = double.Parse((worksheet.Cells[id, 7].Value.ToString()));
                double step4 = double.Parse((worksheet.Cells[id, 8].Value.ToString()));
                double step5 = double.Parse((worksheet.Cells[id, 9].Value.ToString()));
                double step6 = double.Parse((worksheet.Cells[id, 10].Value.ToString()));
                //Console.WriteLine($"{orderId} \t {orderName} \t {quantity} \t {expectCompleteTime} \t {step1} \t {step2} \t {step3} \t {step4} \t {step5} \t {step6}");
                result.Add(new Order(orderId, orderName, new List<Step>() { new Step(step1, 0), new Step(step2, 2), new Step(step3, 3), new Step(step4, 4), new Step(step5, 5), new Step(step6, 6) }, expectCompleteTime));
                currentRow++;
            }
        }
        return result;
    }
    public static void WriteExcelWithEPPlus(string filePath, int numberOfMachines, List<Order> firstOrderList, List<Order> bestOrderList)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            // Create a new worksheet
            var worksheet = package.Workbook.Worksheets.Add("FirstOrder");
            worksheet.Cells[1, 1].Value = "Total Tardiness";
            worksheet.Cells[1, 2].Value = SimulatedAnnealingAlgV2.CalculateOverdueTime(firstOrderList);
            ExtenstionsStation[,] extenstionsStations = WriteOutLine(package, worksheet, numberOfMachines);
            WriteStationProcessOrder(package, worksheet, numberOfMachines, firstOrderList, extenstionsStations);
            // Create a new worksheet
            worksheet = package.Workbook.Worksheets.Add("BestOrder");
            worksheet.Cells[1, 1].Value = "Total Tardiness";
            worksheet.Cells[1, 2].Value = SimulatedAnnealingAlgV2.CalculateOverdueTime(bestOrderList);
            extenstionsStations = WriteOutLine(package, worksheet, numberOfMachines);
            WriteStationProcessOrder(package, worksheet, numberOfMachines, bestOrderList, extenstionsStations);

            // Save the new workbook

                string projectLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string projectDir = Path.GetDirectoryName(projectLocation);
                string projectPath = string.Join("\\", projectDir.Split('\\').Take(projectDir.Split('\\').Length - 4)) + "\\Result\\Result.xlsx";

                package.SaveAs(new FileInfo(projectPath));
        }
    }
    public static ExtenstionsStation[,] WriteOutLine(ExcelPackage package, ExcelWorksheet worksheet, int numberOfMachines)
    {
        int plus = 0;
        if (numberOfMachines % 2 != 0) plus++;
        int iRows = 1;
        worksheet.Cells[$"A{plus + 2 + numberOfMachines / 2}"].Value = "Spinning machine";
        worksheet.Cells[$"A{plus + 2 + 1 + numberOfMachines * 1 + numberOfMachines / 2}"].Value = "Prod68 1";
        worksheet.Cells[$"A{plus + 2 + 2 + numberOfMachines * 2 + numberOfMachines / 2}"].Value = "Prod68 2";
        worksheet.Cells[$"A{plus + 2 + 3 + numberOfMachines * 3 + numberOfMachines / 2}"].Value = "Prod59";
        worksheet.Cells[$"A{plus + 2 + 4 + numberOfMachines * 4 + numberOfMachines / 2}"].Value = "Sizing, ironing";
        worksheet.Cells[$"A{plus + 2 + 5 + numberOfMachines * 5 + numberOfMachines / 2}"].Value = "Packaging";
        ExtenstionsStation[,] extenstionsStation = new ExtenstionsStation[6, numberOfMachines];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < numberOfMachines; j++)
            {
                iRows++;
                if (j == 0) iRows++;
                worksheet.Cells[iRows, 2].Value = j + 1;
                extenstionsStation[i, j] = new ExtenstionsStation(iRows,3);
            }
        }
        return extenstionsStation;
    }
    public static void WriteStationProcessOrder(ExcelPackage package, ExcelWorksheet worksheet, int numberOfMachines, List<Order> orderList, ExtenstionsStation[,] extenstionsStation)
    {
        foreach (Order order in orderList) 
        {
            for (int i = 0; i < order.Steps.Count; i++)
            {
                if (order.Steps[i].RequireTime != 0)
                {
                    worksheet.Cells[extenstionsStation[i, order.Steps[i].MachineProcess].Row, extenstionsStation[i, order.Steps[i].MachineProcess].Column++].Value = order.Id;
                    //Debug.WriteLine($"Order{order.Id} -> Step {i + 1} -> Duration {order.Steps[i].RequireTime}");
                }
            }
        }
    }
    public static List<int> GetOrderOrders(string filePath)
    {
        List<int> result = new List<int>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            // Start reading data from a specific row (replace 1 with your starting row)
            int currentRow = 1;

            while (true)
            {
                if (worksheet.Cells[currentRow, 1].Value == null)
                {
                    break;
                }
                // Read data from each cell in the current row
                int orderId = int.Parse(worksheet.Cells[currentRow, 14].Value.ToString());
                result.Add(orderId);
                currentRow++;
            }
        };

        return result;
    }
}

public class ExtenstionsStation
{
    public int Row { get; set; }
    public int Column { get; set; }
    public ExtenstionsStation(int Row, int Column) 
    {
        this.Row = Row;
        this.Column = Column;
    }

    public override string? ToString()
    {
        return $"Row: {Row} - Column: {Column}";
    }
}