using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WealthManagementAssessment.Domain;

namespace WealthManagementAssessment.WealthManagementService
{
    public class AssetValuationService
    {


        static string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\Investments.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\Transactions.csv");

        List<Investment> selectedInvestments = new List<Investment>();
        List<Transaction> selectedEstate = new List<Transaction>();
        List<Transaction> selectedBuilding = new List<Transaction>();

        public string OwnerId { get; set; }

        public AssetValuationService(string ownerId)
        {
            OwnerId = ownerId;
        }


        //calculate totalInvestments
        public double RealStateEngine()
        {
            bool firstLine = true;

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

                    if (fields[0] == OwnerId)
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

            return selectedInvestments.Count;

        }
    
        
        //RealStateEngine
        public double Engine()
        {
            return RealStateEngine();
        }
    
    }


}
