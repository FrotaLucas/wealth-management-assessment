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

        public DateTime DateTime { get; set; }

        public AssetValuationService(string ownerId)
        {
            OwnerId = ownerId;
        }


        //calculate totalInvestments
        public double TotalInvestments()
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
                    }


                    count++;
                }

            }

            return selectedInvestments.Count;

        }
    
        
        public double RealStateEngine()
        {
            bool firstLine = false;

            using (var reader = new StreamReader(fileTransactions))
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

                    foreach (var investment in selectedInvestments)
                    {
                        //check Estate Value
                        if (investment.InvestmentId == fields[0] && fields[1] == "Estate")
                        {
                            var transaction = new Transaction();
                            transaction.InvestmentId = investment.InvestmentId; //or fields[0]
                            transaction.Type = fields[1];
                            transaction.Value = Double.Parse(fields[3]);
                            transaction.DateTime = DateTime.Parse(fields[2]);

                            selectedEstate.Add(transaction);
                            count++;
                        }

                        //check Building Value
                        if (investment.InvestmentId == fields[0] && fields[1] == "Building")
                        {
                            var transaction = new Transaction();
                            transaction.InvestmentId = investment.InvestmentId; //or fields[0]
                            transaction.Type = fields[1];
                            transaction.Value = Double.Parse(fields[3]);
                            transaction.DateTime = DateTime.Parse(fields[2]);

                            selectedBuilding.Add(transaction);
                        }
                    }


                }

            }

            foreach(var realState in selectedEstate)
            {
                Console.WriteLine(realState.DateTime);
            }

            return selectedBuilding.Count;
        }

    }


}
