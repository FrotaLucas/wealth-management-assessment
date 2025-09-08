using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IFilesReader _filesReader;


        //public Dictionary<int investorId, List<Investment>> dictinary = new Dictionary();

        public Investor Investor { get; set; } = new Investor();

        public AssetRepository(IFilesReader filesReader)
        {
            _filesReader = filesReader;
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

            List<Quote> quoteOfInvestor = _filesReader.ReadQuotes(investments, valuationDate);


            double stockSumup = 0;
            foreach (var investment in stockInvestments)
            {

                double totalShares = investment.Transactions.Sum( transaction => transaction.Value);

                //cuidado se valuationDate for Muito ALTO ou MUITO baixo da erro 
                var quoteToday = quoteOfInvestor
                    .FirstOrDefault(quote => quote.Date <= valuationDate && quote.ISIN == investment.ISIN);

                // E QUANTO o ARQUIVO csv NAO TIVER A  cotacao que eu procuro ???
                //all quotes older than valuationDate are no available
                //if (quoteToday == null)
                //    quoteToday = QuotesOfInvestor
                //        .Where(quote => quote.Isin == investment.Isin)
                //        .LastOrDefault();


                var marketValue = totalShares * quoteToday.PricePerShare;
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
            DateTime t1 = DateTime.Now;
            Dictionary<string, List<Investment>> dictionary = _filesReader.GetDictionary( ownerId, valuationDate);
            DateTime t2 = DateTime.Now;
            Console.WriteLine($"return Dictionary: {t2- t1}");

            foreach (var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum( t => t.Value );

                //old code
                //List<Investment> investmentsOfFound = allInvestments.Where(i => i.InvestorId == fond.FondsInvestor).ToList();
                //_filesReader.ReadTransactions(investmentsOfFound, valuationDate);

                //new code
                //DateTime t3 = DateTime.Now;
                dictionary.TryGetValue(fond.FondsInvestor, out var investmentsOfFound);
                //DateTime t4 = DateTime.Now;
                //Console.WriteLine($"try Get Dictionary: {t4-t3}");

                //DateTime t5 = DateTime.Now;
                double realStateSumup = RealStateEngine(investmentsOfFound);
                //DateTime t6 = DateTime.Now;
                //Console.WriteLine($"real state engine: {t6 - t5}");

                //DateTime t7 = DateTime.Now;
                double stockSumup = StockEngine(investmentsOfFound, valuationDate);
                //DateTime t8 = DateTime.Now;
                //Console.WriteLine($"stockEngine {t8-t7}");


                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }
            Console.WriteLine("total asset value of fonds: " + fondSumup);

            return fondSumup;
        }
    }
}
