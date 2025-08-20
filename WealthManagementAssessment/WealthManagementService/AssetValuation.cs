using WealthManagementAssessment.Domain;

namespace WealthManagementAssessment.WealthManagementService
{
    public class AssetValuation
    {

        static string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\InvestmentsT.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\TransactionsT.csv");
        string fileQuotes = Path.Combine(baseDirectory, "Csv\\Quotes.csv");

        public double RealStateSumup { get; set; }

        public double StockSumup { get; set; }

        public double FondSumup { get; set; }

        public string OwnerId { get; set; }

        //EndDate or ReferenceDate?
        public DateTime ValuationDate { get; set; }

        public Investor Investor { get; set; } = new Investor();

        //chose a better name
        public List<Quote> QuotesOfInvestor { get; set; } = new List<Quote>();

        public AssetValuation(string ownerId, DateTime valuationDate)
        {
            OwnerId = ownerId;
            ValuationDate = valuationDate;
        }
        
        public List<Investment> ReadInvestments(string ownerId)
        {
            var investments = File.ReadLines(fileInvestments)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(parts => parts[0] == ownerId)
                .OrderByDescending(parts => parts[1]) //92.. 82.. 81.. 
                .Select(parst => new Investment
                {
                    InvestmentId = parst[1],
                    InvestorId = parst[0],
                    InvestmentType = parst[2],
                    Isin = parst[3],
                    City = parst[4],
                    FondsInvestor = parst[5]
                }).ToList();

            Console.WriteLine($"total investment of investor90: {investments.Count}");

            return investments;
        }

        public void ReadTransactions(List<Investment> investments)
        {
            foreach (var investment in investments)
            {
                investment.Transactions = File.ReadLines(fileTransactions)
                    .Skip(1)
                    .Select(line => line.Split(";"))
                    .Where(parts => parts[0] == investment.InvestmentId && DateTime.Parse(parts[2]) < ValuationDate) //cut out future transactions
                    .Select(parts => new Transaction
                    {
                        InvestmentId = parts[0],
                        Type = parts[1],
                        Date = DateTime.Parse(parts[2]),
                        Value = Double.Parse(parts[3])

                    }).ToList();
            }
        }

        private List<Quote> ReadQuotes(List<Investment> investments)
        {
            var quotes = new List<Quote>();

            //Storing Quotes
            foreach (var investment in investments)
            {
                var invQuotes = File.ReadLines(fileQuotes)
                    .Skip(1)
                    .Select(parts => parts.Split(";"))
                    .Where(parts => parts[0] == investment.Isin && DateTime.Parse(parts[1]) < ValuationDate) //cut out unused quote range
                    .OrderByDescending(parts => parts[1])
                    .Select(parts => new Quote
                    {
                        Isin = parts[0],
                        Date = DateTime.Parse(parts[1]),
                        PricePerShare = float.Parse(parts[2])

                    }).ToList();

                quotes.AddRange(invQuotes);
            }

            Console.WriteLine($"Total Quotes: {quotes.Count}");


            int count = investments.Sum(i => i.Transactions.Count);
            Console.WriteLine($"Finish totaltransactions: {count}");

            return quotes;
        }

        public void FilesReader()
        {
            //read investment
            Investor.Investments = ReadInvestments(OwnerId);
            //read transactions
            ReadTransactions(Investor.Investments);
            //read quotes of Investor
            //QuotesOfInvestor = ReadQuotes(Investor.Investments);

        }

        
        public void RealStateEngine(List<Investment> investments)
        {
            RealStateSumup = 0;
            RealStateSumup = investments
                .Where(investment => investment.InvestmentType == "RealEstate")
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            Console.WriteLine($"Sumup RealEstate: {RealStateSumup}");

        }

        public void StockEngine(List<Investment> investments)
        {
            StockSumup = 0;

            foreach (var investment in investments)
            {
                double totalShares = 0;

                if (string.IsNullOrEmpty(investment.Isin))
                    continue;


                foreach (var transaction in investment.Transactions)
                {
                    var quote = QuotesOfInvestor
                        .FirstOrDefault(quote => quote.Date <= transaction.Date && quote.Isin == investment.Isin);

                    if (quote == null)
                        quote = QuotesOfInvestor.LastOrDefault();

                    totalShares += (int)Math.Round(transaction.Value / quote.PricePerShare);

                }

                //get quote of today to calculate Asset
                var quoteToday = QuotesOfInvestor
                    .FirstOrDefault(quote => quote.Date <= ValuationDate && quote.Isin == investment.Isin);


                if (quoteToday == null)
                    quoteToday = QuotesOfInvestor.LastOrDefault();

                //Console.WriteLine($"Price share today:   {quoteToday.PricePerShare}");

                //var todaysValue = 10;
                var marketValue = totalShares * quoteToday.PricePerShare;
                Console.WriteLine($"value for stocks:   {marketValue}");
                StockSumup += marketValue;
            }
            Console.WriteLine($"Sumup Stocks:   {StockSumup}");

        }



        public void FondEngineV2()
        {
            FondSumup = 0;
            var fonds = Investor.Investments
                .Where(i => i.InvestmentType == "Fonds")
                .ToList();

            //Console.WriteLine(fonds[4].InvestmentId);

            foreach( var fond in fonds)
            {
                //realstate
                var investments = ReadInvestments(fond.FondsInvestor);


                //stocks
            }
        }

        public void FondEngine()
        {
            //filter only fonds of investor
            List<Investor> fonds = Investor.Investments
                .Where(investment => investment.InvestmentType == "Fonds")
                .Select(investment => new Investor
                {
                    InvestorId = investment.FondsInvestor

                })
                .ToList();


            var fond1 = fonds[0];

            //CODIGO REPETIDO
            //reading investment of 1. Fond
            fond1.Investments = File.ReadLines(fileInvestments)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(parts => parts[0] == fond1.InvestorId)
                .OrderByDescending(parts => parts[1]) //92.. 82.. 81.. 
                .Select(parst => new Investment
                {
                    InvestmentId = parst[1],
                    InvestorId = parst[0],
                    InvestmentType = parst[2],
                    Isin = parst[3],
                    City = parst[4],
                    FondsInvestor = parst[5]
                }).ToList();

            //CODIGO REPETIDO
            //reading transactions for fond1
            foreach (var investment in fond1.Investments)
            {
                investment.Transactions = File.ReadLines(fileTransactions)
                    .Skip(1)
                    .Select(line => line.Split(";"))
                    .Where(parts => parts[0] == investment.InvestmentId && DateTime.Parse(parts[2]) < ValuationDate) //cut out future transactions
                    .Select(parts => new Transaction
                    {
                        InvestmentId = parts[0],
                        Type = parts[1],
                        Date = DateTime.Parse(parts[2]),
                        Value = Double.Parse(parts[3])

                    }).ToList();
            }


            //CODIGO REPETIDO
            //Reading quote for fond1
            List<Quote> QuoteofFond1 = new List<Quote>();
            foreach (var investment in fond1.Investments)
            {
                var quotes = File.ReadLines(fileQuotes)
                    .Skip(1)
                    .Select(parts => parts.Split(";"))
                    .Where(parts => parts[0] == investment.Isin && DateTime.Parse(parts[1]) < ValuationDate) //cut out unused quote range
                    .OrderByDescending(parts => parts[1])
                    .Select(parts => new Quote
                    {
                        Isin = parts[0],
                        Date = DateTime.Parse(parts[1]),
                        PricePerShare = float.Parse(parts[2])

                    }).ToList();

                QuoteofFond1.AddRange(quotes);
            }


            //use RealStateEngine for fond1 and calculate new RealStateSumup
            RealStateEngine();
            //use StockEngine for fond1 and calculate new StockSumup
            StockEngine();
            
            var percentageOfFond1 = Investor.Investments
                .Where(investement => investement.FondsInvestor == fond1.InvestorId)
                .SelectMany(investment => investment.Transactions)
                .Where(transaction => transaction.Date < ValuationDate)
                .Sum(transaction => transaction.Value);

            Console.WriteLine($"percentage total: {percentageOfFond1}");

            var assetOfFond1 = percentageOfFond1 * (StockSumup + RealStateSumup);



            //continue ... fonds[1] , fonds[2], fonds[3]


            //sum of all funds of Investor OwnerId
            //FondSumup = assetOfFond1 + assetOfFond2 + ...

            Console.WriteLine($"total fonds: {fonds.Count}");
        }


        public void AssetEngine()
        {
            //RealState
            RealStateEngine(Investor.Investments);

            //Stocks
            StockEngine(Investor.Investments);
            //Fonds
        }
    }
}
