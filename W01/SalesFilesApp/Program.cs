using System.Text;

string directoryPath = "salesdata";
Directory.CreateDirectory(directoryPath);

// Sample sales files
File.WriteAllText(Path.Combine(directoryPath, "january.txt"), "1500.50");
File.WriteAllText(Path.Combine(directoryPath, "february.txt"), "2750.75");
File.WriteAllText(Path.Combine(directoryPath, "march.txt"), "3200.25");

GenerateSalesSummary(directoryPath);

void GenerateSalesSummary(string directory)
{
    string[] files = Directory.GetFiles(directory);

    decimal totalSales = 0;

    StringBuilder report = new StringBuilder();

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");

    List<string> details = new List<string>();

    foreach (string file in files)
    {
        string fileName = Path.GetFileName(file);

        string content = File.ReadAllText(file);

        if (decimal.TryParse(content, out decimal salesAmount))
        {
            totalSales += salesAmount;

            details.Add($"{fileName}: {salesAmount:C}");
        }
    }

    report.AppendLine($"Total Sales: {totalSales:C}");
    report.AppendLine();
    report.AppendLine("Details:");

    foreach (string detail in details)
    {
        report.AppendLine(detail);
    }

    string reportPath = Path.Combine(directory, "SalesSummary.txt");

    File.WriteAllText(reportPath, report.ToString());

    Console.WriteLine("Sales summary report created!");
    Console.WriteLine(report.ToString());
}