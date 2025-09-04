using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IFilesReader _filesReader;



        //public Dictionary<int investorId, List<Investment>> dictinary = new Dictionary();


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
            Investor.Investments = _filesReader.ReadInvestmentByInvestor(ownerId);

            _filesReader.ReadTransactions(Investor.Investments, valuationDate);

            //read quotes of Investor
            QuotesOfInvestor = _filesReader.ReadQuotes(Investor.Investments, valuationDate);

        }

        public List<Investment> GetAllInvestments(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _filesReader.ReadInvestmentByInvestor(ownerId);

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
            //50k * 250k

            var start = DateTime.Now;

            QuotesOfInvestor = _filesReader.ReadQuotes(investments, valuationDate);

            Console.WriteLine($" reade quotes: {DateTime.Now  - start}"); // data e hora local

            //var dic = new Dictionary<>;
            // como transformar uma lista de objetos em diciionario 
            

            double stockSumup = 0;
            foreach (var investment in investments)
            {
                double totalShares = 0;

                if (string.IsNullOrEmpty(investment.ISIN))
                    continue;

                //A stocks in euro amount
                //foreach (var transaction in investment.Transactions)
                //{
                //    var quote = QuotesOfInvestor
                //        .FirstOrDefault(quote => quote.Date <= transaction.Date && quote.ISIN == investment.ISIN);

                //    // E QUANTO o ARQUIVO csv NAO TIVER A  cotacao que eu procuro ???
                //    // vai buscar uma cotacao aleatorio ?

                //    if (quote == null)
                //        quote = QuotesOfInvestor.LastOrDefault();

                //    //Console.WriteLine(  $"volume trasaction : {transaction.Value}  quote: {quote.PricePerShare}\n");
                //    totalShares += (int)Math.Round(transaction.Value / quote.PricePerShare);

                //    //Console.WriteLine($"rounded value {Math.Round(transaction.Value / quote.PricePerShare)}" );
                //}


                //B stocks in shares
                totalShares = investment.Transactions.Sum( transaction => transaction.Value);   



                //get quote of today to calculate Asset
                var quoteToday = QuotesOfInvestor
                    .FirstOrDefault(quote => quote.Date <= valuationDate && quote.ISIN == investment.ISIN);
                
                //all quotes older than valuationDate are no available
                //if (quoteToday == null)
                //    quoteToday = QuotesOfInvestor
                //        .Where(quote => quote.Isin == investment.Isin)
                //        .LastOrDefault();

                //Console.WriteLine($"Price share today:   {quoteToday.PricePerShare}");

                var marketValue = totalShares * quoteToday.PricePerShare;
                //Console.WriteLine($"value for stocks:   {marketValue}");
                stockSumup += marketValue;
            }
            Console.WriteLine($"Sumup Stocks:   {stockSumup}");

            return stockSumup;
        }

        public double FondEngine(string ownerId, DateTime valuationDate)
        {
            double fondSumup = 0;

            //old code
            //var fonds = Investor.Investments
            //    .Where(i => i.InvestmentType == "Fonds")
            //    .ToList();


            //new code
            List<Investment> investments = _filesReader.ReadInvestments();

            var fonds = investments
                .Where(investment => investment.InvestorId == ownerId && investment.InvestmentType == "Fonds" )
                .ToList();

            _filesReader.ReadTransactions(fonds, valuationDate);

            foreach( var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum( t => t.Value );

                //old code
                //List<Investment> investmentsOfFound = _filesReader.ReadInvestmentByInvestor(fond.FondsInvestor);
                //new code
                List<Investment> investmentsOfFound = investments.Where(i => i.InvestorId == fond.FondsInvestor).ToList();

                _filesReader.ReadTransactions(investmentsOfFound, valuationDate);

                double realStateSumup = RealStateEngine(investmentsOfFound);

                double stockSumup = StockEngine(investmentsOfFound, valuationDate);

                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }
            Console.WriteLine("total asset value of fonds: " + fondSumup);

            return fondSumup;
        }
    }
}
