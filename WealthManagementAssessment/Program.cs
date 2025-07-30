

class Program
{

    static void Main(string[] args)
    {

        Console.WriteLine("start project");


        string path = "C:\\MyFolders\\Visual_Studio_Projects\\WealthManagementAssessment\\WealthManagementAssessment\\Csv\\Investments.csv";

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