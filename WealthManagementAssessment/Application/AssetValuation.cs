using WealthManagementAssessment.Domain;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.WealthManagementService
{
    public class AssetValuation
    {

        static string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\InvestmentsT.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\TransactionsT.csv");
        string fileQuotes = Path.Combine(baseDirectory, "Csv\\Quotes.csv");

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

        
        public double RealStateEngine(List<Investment> investments)
        {
            
            var realStateSumup = investments
                .Where(investment => investment.InvestmentType == "RealEstate")
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            Console.WriteLine($"Sumup RealEstate: {realStateSumup}");

            return realStateSumup;
        }

        public double StockEngine(List<Investment> investments)
        {
            double stockSumup = 0;
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
                stockSumup += marketValue;
            }
            Console.WriteLine($"Sumup Stocks:   {stockSumup}");

            return stockSumup;
        }

        public double FondEngine()
        {
            double fondSumup = 0;
            var fonds = Investor.Investments
                .Where(i => i.InvestmentType == "Fonds")
                .ToList();

            //Console.WriteLine(fonds[4].InvestmentId);

            foreach( var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum( t => t.Value );
                List<Investment> listOfinvestments = ReadInvestments(fond.FondsInvestor);
                ReadTransactions(listOfinvestments);

                //total Asset for realstate
                double realStateSumup = RealStateEngine(listOfinvestments);

                //total Asset for stocks
                double stockSumup = StockEngine(listOfinvestments);

                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }
            Console.WriteLine("total asset value of fonds: " + fondSumup);

            return fondSumup;
        }

        public void AssetEngine()
        {
            //RealState
            //RealStateEngine(Investor.Investments);

            //Stocks
            //StockEngine(Investor.Investments);

            //Fonds
            FondEngine();
        }
    }
}
