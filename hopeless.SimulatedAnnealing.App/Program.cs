using OfficeOpenXml;

public class Program
{
    static Random random = new Random();
    static int currentTime = 0;
    static List<Order> bestSolution;

    //public static void Main1(string[] args)
    //{
    //    List<Order> orders = new List<Order>
    //    {
    //        new Order(1, new List<Step>() { new Step(9), new Step(1), new Step(0), new Step(3) }, 7),
    //        new Order(2, new List<Step>() { new Step(0), new Step(2), new Step(6), new Step(4) }, 13),
    //        new Order(3, new List<Step>() { new Step(7), new Step(4), new Step(3), new Step(0) }, 17),
    //        new Order(4, new List<Step>() { new Step(5), new Step(2), new Step(0), new Step(3) }, 10),
    //        new Order(5, new List<Step>() { new Step(0), new Step(2), new Step(12), new Step(4) }, 14),
    //        new Order(6, new List<Step>() { new Step(4), new Step(8), new Step(3), new Step(0) }, 10),
    //        new Order(0, new List<Step>() { new Step(10), new Step(7), new Step(4), new Step(5) }, 10),
    //    };

    //    double initialTemperature = 1000;
    //    double coolingRate = 0.003;
    //    SimulatedAnnealing sa = new SimulatedAnnealing(orders, initialTemperature, coolingRate);
    //    List<Order> bestOrder = sa.FindBestSolution();

    //    Console.WriteLine("Best order: ");
    //    Console.Write("Orders: "); bestOrder.ForEach(order => Console.Write(order.Id \t "\t"));
    //}

    public static void Main(string[] args)
    {
        List<Order> orders = ReadExcel();
        //List<Order> orders = new List<Order>{
        //    new Order(1, new List<Step>() { new Step(9), new Step(1), new Step(0), new Step(3), new Step(3), new Step(3) }, 7),
        //    new Order(2, new List<Step>() { new Step(0), new Step(2), new Step(6), new Step(4), new Step(6), new Step(4) }, 13),
        //    new Order(3, new List<Step>() { new Step(7), new Step(4), new Step(3), new Step(0), new Step(3), new Step(0) }, 17),
        //    new Order(4, new List<Step>() { new Step(5), new Step(2), new Step(0), new Step(3), new Step(0), new Step(3) }, 10),
        //    new Order(5, new List<Step>() { new Step(0), new Step(2), new Step(12), new Step(4), new Step(12), new Step(4) }, 14),
        //    new Order(6, new List<Step>() { new Step(4), new Step(8), new Step(3), new Step(0), new Step(3), new Step(0) }, 10),
        //    new Order(0, new List<Step>() { new Step(10), new Step(7), new Step(4), new Step(5), new Step(4), new Step(5) }, 10),
        //};

        double initialTemperature = 1000;
        double coolingRate = 0.003;
        SimulatedAnnealing sa = new SimulatedAnnealing(orders, initialTemperature, coolingRate, 2);
        List<Order> bestOrder = sa.FindBestSolution();

        Console.WriteLine("Best order: ");
        Console.Write("Orders: "); bestOrder.ForEach(order => Console.Write(order.Id + "\t"));
        Console.ReadLine();
        DrawGanttChart(bestOrder);
    }

    static List<Order> ReadExcel()
    {
        List<Order> result = new List<Order> ();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        string filePath = @"C:\Users\mical\Downloads\B.xlsx";
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
                double step2 = double.Parse((worksheet.Cells[currentRow,6].Value.ToString()));
                double step3 = double.Parse((worksheet.Cells[currentRow,7].Value.ToString()));
                double step4 = double.Parse((worksheet.Cells[currentRow,8].Value.ToString()));
                double step5 = double.Parse((worksheet.Cells[currentRow,9].Value.ToString()));
                double step6 = double.Parse((worksheet.Cells[currentRow,10].Value.ToString()));
                //Console.WriteLine($"{orderId} \t {orderName} \t {quantity} \t {expectCompleteTime} \t {step1} \t {step2} \t {step3} \t {step4} \t {step5} \t {step6}");
                result.Add(new Order(orderId, orderName, new List<Step>() {new Step(step1), new Step(step2), new Step(step3), new Step(step4), new Step(step5), new Step(step6)}, expectCompleteTime));
                currentRow++;
            }
        }
        return result;
    }
    public static void DrawGanttChart(List<Order> orders)
    {
        Console.WriteLine("Gantt Chart:");

        // Calculate total processing time
        double totalProcessingTime = 0;
        foreach (var order in orders)
        {
            totalProcessingTime += (order.CompleteTime - order.StartTime);
        }

        // Define the width of the chart
        int chartWidth = 60;

        // Calculate scale factor to fit the chart within console width
        double scaleFactor = (double)chartWidth / totalProcessingTime;

        // Draw chart header
        Console.WriteLine($"{"Order",-10} | {"Gantt Chart",-60}");
        Console.WriteLine(new string('-', 72));

        // Draw Gantt chart for each order
        foreach (var order in orders)
        {
            // Calculate the number of characters to represent processing time
            int numChars = (int)((order.CompleteTime - order.StartTime) * scaleFactor);

            // Draw order ID
            Console.Write($"{order.Id,-10} | ");

            // Draw Gantt chart bar with color
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('#', numChars));
            Console.ResetColor(); // Reset color

            // Add padding if necessary
            Console.WriteLine(new string(' ', chartWidth - numChars));
        }
    }
}