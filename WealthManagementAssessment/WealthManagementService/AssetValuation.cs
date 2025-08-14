using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void FilesReader()
        {

            Investor.Investments = File.ReadLines(fileInvestments)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(parts => parts[0] == OwnerId)
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

            Console.WriteLine($"total investment of investor90: {Investor.Investments.Count}");

            var trans = new Transaction();

            foreach (var investment in Investor.Investments)
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

            //Storing Quotes
            foreach (var investment in Investor.Investments)
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

                QuotesOfInvestor.AddRange(quotes);
            }

            //Console.WriteLine($"Total Quotes: {QuotesOfInvestor.Count}");

            //foreach (var investment in Investor.Investments)
            //{
            //    Console.WriteLine($"investmentId: {investment.InvestmentId}\n");
            //    Console.WriteLine($"investorId: {investment.InvestorId}\n");
            //    Console.WriteLine($"ISIN: {investment.Isin}\n");
            //    Console.WriteLine($"invesment Type: {investment.InvestmentType}\n");
            //    Console.WriteLine($"City: {investment.City} \n");
            //}

            //foreach (var investment in Investor.Investments)
            //{
            //    Console.WriteLine($"ISIN:{investment.Isin}");
            //    foreach (var transaction in investment.Transactions)
            //    {
            //        Console.WriteLine($"investment Id: {transaction.InvestmentId}\n");
            //        Console.WriteLine($"Type: {transaction.Type}\n");
            //        Console.WriteLine($"Value: {transaction.Value}\n");
            //        Console.WriteLine($"Datetime: {transaction.Date} \n");
            //    }
            //}


            //foreach (var quotes in QuotesOfInvestor)
            //{
            //    Console.WriteLine($"ISIN: {quotes.Isin}\n");
            //    Console.WriteLine($"Date ISIN: {quotes.Date}\n");

            //}

            int count = Investor.Investments.Sum(i => i.Transactions.Count);
            Console.WriteLine($"Finish totaltransactions: {count}");
        }

        public void RealStateEngine()
        {
            RealStateSumup = 0;
            RealStateSumup = Investor.Investments
                .Where(investment => investment.InvestmentType == "RealEstate")
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);
            //.SelectMany(investment => investment.Transactions)
            //.Where(Investment => Investment.Type == "Estate")
            //.Sum(transaction => transaction.Value);

            Console.WriteLine($"Sumup RealEstate: {RealStateSumup}");

        }

        public void StockEngine()
        {
            //StockSumup = Investor.Investments
            //     .Where(investment => investment.InvestmentType == "Stock")
            //     .SelectMany(investment => investment.Transactions)
            //     .Sum(transaction => transaction.Value / 10);
            StockSumup = 0;

            foreach (var investment in Investor.Investments)
            {
                double totalShares = 0;

                Console.WriteLine($"Isin of investment:{investment.Isin}");
                //NAO SAO TODOS investment que tem ISIN. nao seria melhor filtrar antes somente os stocks?
                if (string.IsNullOrEmpty(investment.Isin))
                    continue;


                foreach (var transaction in investment.Transactions)
                {
                    var quote = QuotesOfInvestor
                        .FirstOrDefault(quote => quote.Date <= transaction.Date && quote.Isin == investment.Isin);

                    if (quote == null)
                        quote = QuotesOfInvestor.LastOrDefault();

                    //Console.WriteLine($"data escolhida: {quote.Date}");


                    //Console.WriteLine($"bought or sold shares:    {(int)Math.Round(transaction.Value / quote.PricePerShare)}");

                    totalShares += (int)Math.Round(transaction.Value / quote.PricePerShare);

                    //Console.WriteLine($"total shares updated:   {totalShares}");

                }

                //Console.WriteLine($"Fiinal total shares today:   {totalShares}");


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


        public void FondEngine()
        {
            List<Investor> fonds = Investor.Investments
                .Where(investment => investment.InvestmentType == "Fonds")
                .Select(investment => new Investor
                {
                    InvestorId = investment.FondsInvestor

                })
                .ToList();


            var fond1 = fonds[0];

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

            //reading transations for fond1
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

            //RealStateEngine();
            //StockEngine();

            var percentageReal = Investor.Investments
                .Where(investement => investement.FondsInvestor == fond1.InvestorId)
                .SelectMany(investment => investment.Transactions)
                .Where(transaction => transaction.Date < ValuationDate)
                .Sum(transaction => transaction.Value);

            Console.WriteLine($"percentage total: {percentageReal}");

            //var assetRealState =  RealStateSumup

            //continue ... fonds[1] , fonds[2], fonds[3]


            Console.WriteLine($"total fonds: {fonds.Count}");
        }

    }
}
