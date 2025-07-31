using System.Diagnostics;
using System.IO;
using WealthManagementAssessment.Domain;

class Program
{

    private static void Main(string[] args)
    {

        string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string folderCsv = "Csv\\Investments.csv";

        string fileInvestments = Path.Combine(baseDirectory, folderCsv);
        string investorId = "Investor30";
        bool firstLine = true;


        var selectedInvestments = new List<Investment>();
        var selectedTransactions =  new List<Transaction>();

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

                if( fields[0] == investorId)
                {
                    var investment = new Investment();

                    investment.InvestorId = fields[0];
                    investment.InvestmentId = fields[1];
                    investment.InvestmentType = fields[2]; //new code

                    selectedInvestments
                        .Add(  investment);
                }


                count++;
                Console.WriteLine($"id: {investor}");
            }

          

            Console.WriteLine($"total: {selectedInvestments.Count}");
        }
    }

}