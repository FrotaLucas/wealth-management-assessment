using System.Diagnostics;
using System.IO;
using WealthManagementAssessment.Domain;

class Program
{

    private static void Main(string[] args)
    {

        string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));

        string fileInvestments = Path.Combine(baseDirectory, "Csv\\Investments.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\Transactions.csv");

        string investorId = "Investor30";
        bool firstLine = true;


        var selectedInvestments = new List<Investment>();
        var selectedEstate = new List<Transaction>();
        var selectedBuilding = new List<Transaction>();

        using (var reader = new StreamReader(fileInvestments))
        {
            string? line;
            int count = 0;

            while ((line = reader.ReadLine()) != null)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                var fields = line.Split(';');
                var investor = fields[0];

                if (fields[0] == investorId)
                {
                    var investment = new Investment();

                    investment.InvestorId = fields[0];
                    investment.InvestmentId = fields[1];
                    investment.InvestmentType = fields[2]; //new code
                    investment.Isin = fields[3];

                    selectedInvestments
                        .Add(investment);
                    //Console.WriteLine($"id: {investment.InvestorId}");
                }


                count++;
            }


        }


        using (var reader = new StreamReader(fileTransactions))
        {
            string? line;
            int count = 0;
            firstLine = false;
            

            while ((line = reader.ReadLine()) != null)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                var fields = line.Split(';');

                //check Estate Value
                foreach (var investment in selectedInvestments)
                {
                    if(investment.InvestmentId == fields[0] && fields[1] == "Estate")
                    {
                        var transaction = new Transaction();
                        transaction.InvestmentId = investment.InvestmentId; //or fields[0]
                        transaction.Type = fields[1];
                        transaction.Value = Double.Parse(fields[3]);
                        //missing Date
                        
                        selectedEstate.Add(transaction);
                        count++;
                    }

                    if (investment.InvestmentId == fields[0] && fields[1] == "Building")
                    {
                        var transaction = new Transaction();
                        transaction.InvestmentId = investment.InvestmentId; //or fields[0]
                        transaction.Type = fields[1];
                        transaction.Value = Double.Parse(fields[3]);

                        selectedBuilding.Add(transaction);
                    }
                }


            }



        }

        Console.WriteLine($"total Estate for Investor30: {selectedEstate.Count}");
        Console.WriteLine($"total Building for Investor30: {selectedBuilding.Count}");

        foreach (var transactions in selectedEstate)
        {
            Console.WriteLine(transactions.Type.ToString());
        }
    }

}