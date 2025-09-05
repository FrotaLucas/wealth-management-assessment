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

        public List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate)
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

            return realStateSumup;
        }

        public double StockEngine(List<Investment> investments, DateTime valuationDate)
        {
            //50k * 250k
            //var dic = new Dictionary<>;
            // como transformar uma lista de objetos em diciionario 

            var stockInvestments = investments
                .Where( investment => investment.InvestmentType.Equals("Stock") )
                .ToList();

            QuotesOfInvestor = _filesReader.ReadQuotes(investments, valuationDate);


            double stockSumup = 0;
            foreach (var investment in stockInvestments)
            {
                double totalShares = 0;

                //if (string.IsNullOrEmpty(investment.ISIN))
                //    continue;

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



                //cuidado se valuationDate for Muito ALTO ou MUITO baixo da erro 
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

            return stockSumup;
        }

        public double FondEngine(string ownerId, DateTime valuationDate)
        {
            double fondSumup = 0;

            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType == "Fonds")
                .ToList();

            _filesReader.ReadTransactions(fonds, valuationDate);

            //old code
            //List<Investment> allInvestments = _filesReader.ReadInvestments();

            

            //new code
            Dictionary<string, List<Investment>> dictionary = _filesReader.GetDictionary( ownerId, valuationDate);


            foreach (var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum( t => t.Value );

                //old code
                //List<Investment> investmentsOfFound = allInvestments.Where(i => i.InvestorId == fond.FondsInvestor).ToList();
                //_filesReader.ReadTransactions(investmentsOfFound, valuationDate);

                //new code
                dictionary.TryGetValue(fond.FondsInvestor, out var investmentsOfFound);


                double realStateSumup = RealStateEngine(investmentsOfFound);

                double stockSumup = StockEngine(investmentsOfFound, valuationDate);

                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }
            Console.WriteLine("total asset value of fonds: " + fondSumup);

            return fondSumup;
        }
    }
}
