using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

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