using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IFilesReader _filesReader;

        public Investor Investor { get; set; } = new Investor();

        //chose a better name
        public List<Quote> QuotesOfInvestor { get; set; } = new List<Quote>();

        public AssetRepository(IFilesReader filesReader)
        {
            _filesReader = filesReader;
        }

        //dont need that
        public void LoadFilesJustOnce(string ownerId, DateTime valuationDate)
        {
            Investor.Investments = _filesReader.ReadInvestments(ownerId);

            _filesReader.ReadTransactions(Investor.Investments, valuationDate);

            //read quotes of Investor
            QuotesOfInvestor = _filesReader.ReadQuotes(Investor.Investments, valuationDate);

        }

        public List<Investment> GetAllInvestments(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _filesReader.ReadInvestments(ownerId);

            _filesReader.ReadTransactions(investments, valuationDate);

            return investments;
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

        public double StockEngine(List<Investment> investments, DateTime valuationDate)
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
                    .FirstOrDefault(quote => quote.Date <= valuationDate && quote.Isin == investment.Isin);


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

        public double FondEngine(DateTime valuationDate)
        {
            double fondSumup = 0;
            var fonds = Investor.Investments
                .Where(i => i.InvestmentType == "Fonds")
                .ToList();

            //Console.WriteLine(fonds[4].InvestmentId);

            foreach( var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum( t => t.Value );
                List<Investment> listOfinvestments = _filesReader.ReadInvestments(fond.FondsInvestor);
                _filesReader.ReadTransactions(listOfinvestments, valuationDate);

                //total Asset for realstate
                double realStateSumup = RealStateEngine(listOfinvestments);

                //total Asset for stocks
                double stockSumup = StockEngine(listOfinvestments, valuationDate);

                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }
            Console.WriteLine("total asset value of fonds: " + fondSumup);

            return fondSumup;
        }
    }
}
