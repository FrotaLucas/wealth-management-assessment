using System.Diagnostics;
using System.IO;

class Program
{

    private static void Main(string[] args)
    {

        string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string folderCsv = "Csv\\Investments.csv";

        string fileInvestments = Path.Combine(baseDirectory, folderCsv);
        string investmentId = "Investor30";
        bool firstLine = true;

        using (var reader = new StreamReader(fileInvestments))
        {
            string? line;
            int count = 0;

            while( (line = reader.ReadLine()) != null )
            {
                if (firstLine) {
                    firstLine = false;
                    continue;
                }

                var fields = line.Split(';');
                var investor = fields[0];
                Console.WriteLine($"id: {investor}");

                count++;
            }

            Console.WriteLine($"total: {count}");
        }
    }

}