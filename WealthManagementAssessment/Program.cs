using System.Diagnostics;
using System.IO;
using WealthManagementAssessment.Domain;
using WealthManagementAssessment.WealthManagementService;

class Program
{

    private static void Main(string[] args)
    {

        string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));

        //1 snapshot aqui
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\Investments.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\Transactions.csv");

        string investorId = "Investor90";
        bool firstLine = true;


        var selectedInvestments = new List<Investment>();
        var selectedEstate = new List<Transaction>();
        //2 snapshot aqui
        var selectedBuilding = new List<Transaction>();


        var obj = new AssetValuationService(investorId);

        Console.WriteLine("total investments: " + obj.TotalInvestments());

        //using (var reader = new StreamReader(fileInvestments))
        //{
        //    string? line;
        //    int count = 0;

        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        if (firstLine)
        //        {
        //            firstLine = false;
        //            continue;
        //        }

        //        var fields = line.Split(';');
        //        var investor = fields[0];

        //        if (fields[0] == investorId)
        //        {
        //            var investment = new Investment();

        //            investment.InvestorId = fields[0];
        //            investment.InvestmentId = fields[1];
        //            investment.InvestmentType = fields[2]; //new code
        //            investment.Isin = fields[3];

        //            selectedInvestments
        //                .Add(investment);
        //            //Console.WriteLine($"id: {investment.InvestorId}");
        //        }


        //        count++;
        //    }

        //    //3 snapshot aqui
        //    Console.WriteLine($"Total investments: {selectedInvestments.Count}");
        //}

        //4 snapshot aqui
      
        //6 snapshot aqui
        Console.WriteLine($"total Estate for Investor30: {selectedEstate.Count}");
        Console.WriteLine($"total Building for Investor30: {selectedBuilding.Count}");

        //foreach (var transactions in selectedEstate)
        //{
        //    Console.WriteLine(transactions.Type.ToString());
        //}
    }

}