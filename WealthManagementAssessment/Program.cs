using System.Diagnostics;
using System.IO;

class Program
{

    private static void Main(string[] args)
    {

        string BaseDirectory = AppContext.BaseDirectory;
        //string filePath = "\\Csv\\Investments.csv";
        string path = Path.GetFullPath(Path.Combine(BaseDirectory, @"..\..\..\"));

        string folderCsv = "Csv\\Investments.csv";

        path = Path.Combine(path, folderCsv);
        //string path = "C:\\MyFolders\\Visual_Studio_Projects\\WealthManagementAssessment\\WealthManagementAssessment\\Csv\\Investments.csv";

        string investmentId = "Investor30";

        using (var reader = new StreamReader(path))
        {
            string? line;
            int count = 0;

            while( (line = reader.ReadLine()) != null )
            {
                var fields = line.Split(';');
                var investor = fields[0];
                Console.WriteLine($"id: {investor}");

                count++;
            }

            Console.WriteLine($"total: {count}");
        }
    }

}