using WealthManagementAssessment.Domain;

namespace WealthManagementAssessment.WealthManagementService
{       //ValuationMangement or valuationEngine or PortifolioManagement or AssetManagement or AssetValuation
    public class AssetValuationService
    {


        static string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\Investments.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\Transactions.csv");

        List<Investment> selectedInvestments = new List<Investment>();
        List<Transaction> selectedEstate = new List<Transaction>();
        List<Transaction> selectedBuilding = new List<Transaction>();

        List<Transaction> selectedStocks = new List<Transaction>();

        public string OwnerId { get; set; }

        //EndDate or ReferenceDate?
        public DateTime ValuationDate { get; set; }

        public double RealStateSumup { get; set; }

        public double StockSumup { get; set; }

        public AssetValuationService(string ownerId, DateTime dateTime)
        {
            OwnerId = ownerId;
            ValuationDate = dateTime;
        }


        //delete Function!!
        public string ConvertValuationDateToString()
        {
            string date = this.ValuationDate.ToString("yyyy-MM-dd");

            return date;
        }

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
                        investment.InvestmentType = fields[2]; 
                        investment.Isin = fields[3];

                        //not used
                        //RealState
                        //investment.City = fields[4];
                        //Fonds
                        //investment.FondsInvestor = fields[5];

                        selectedInvestments
                            .Add(investment);
                    }


                    count++;
                }

            }

            return selectedInvestments.Count;

        }


        //StockEngine & FondsEngine
        public void RealStateEngine()
        {
            bool firstLine = true;
            RealStateSumup = 0;
            StockSumup = 0;

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
                    var transationDate = DateTime.Parse(fields[2]);

                    foreach (var investment in selectedInvestments)
                    {
                        //check Estate Value
                        if (investment.InvestmentId == fields[0] && fields[1] == "Estate" && ValuationDate > transationDate)
                        {
                            var transaction = new Transaction();
                            transaction.InvestmentId = investment.InvestmentId; //or fields[0]
                            transaction.Type = fields[1];
                            transaction.Value = Double.Parse(fields[3]);
                            transaction.DateTime = DateTime.Parse(fields[2]);

                            selectedEstate.Add(transaction);
                            RealStateSumup += transaction.Value;
                        }

                        //check Building Value
                        if (investment.InvestmentId == fields[0] && fields[1] == "Building" && ValuationDate > transationDate)
                        {
                            var transaction = new Transaction();
                            transaction.InvestmentId = investment.InvestmentId; //or fields[0]
                            transaction.Type = fields[1];
                            transaction.Value = Double.Parse(fields[3]);
                            transaction.DateTime = DateTime.Parse(fields[2]);

                            selectedBuilding.Add(transaction);
                            RealStateSumup += transaction.Value;

                        }
                        DateTime limitDate = DateTime.Parse("12-31-2018");

                        if ("Investment20812" == fields[0] && investment.InvestmentId == "Investment20812" && investment.InvestmentType == "Stock" && ValuationDate < transationDate && transationDate < limitDate)
                        {
                            var transaction = new Transaction();
                            transaction.InvestmentId = investment.InvestmentId;
                            transaction.Type = fields[1];
                            transaction.Value = Double.Parse(fields[3]);
                            transaction.DateTime = DateTime.Parse(fields[2]);

                            selectedStocks.Add(transaction);
                            StockSumup += transaction.Value;

                        }

                        count++;

                    }


                }

            }

            //for (int i = 0; i < selectedBuilding.Count; i++)
            //{
            //    if (selectedBuilding[i].DateTime == ValuationDate)
            //    {
            //        Console.WriteLine("investmentId"+ selectedBuilding[i].InvestmentId);
            //        Console.WriteLine("investmentId"+ selectedEstate[i].InvestmentId);
            //    }
            //}

        }

    }


}
