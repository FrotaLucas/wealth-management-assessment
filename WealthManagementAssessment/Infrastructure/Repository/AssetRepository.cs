using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {
        //private readonly IFilesReader _filesReader;


        //public Dictionary<int investorId, List<Investment>> dictinary = new Dictionary();

        public Dictionary<string, List<Quote>> QuotesByIsin;

        public Dictionary<string, List<Investment>> InvestmetnByOwnerId;

        public Dictionary<string, List<Transaction>> TransactionByInvestmentId;



        public AssetRepository(IFilesReader filesReader)
        {
            //filesReader = filesReader;
            QuotesByIsin = filesReader.ReadQuotesV2();
            InvestmetnByOwnerId = filesReader.ReadInvestmentsv2();
            TransactionByInvestmentId = filesReader.ReadTransactionsV2();
        }

        public List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate)
        {
            //List<Investment> investments = _filesReader.ReadInvestmentByInvestor(ownerId);
            if (!InvestmetnByOwnerId.TryGetValue(ownerId, out var investments))
            {
                Console.WriteLine("INvestmens not found for this investor");
                return new List<Investment>();
            }


            foreach (var investment in investments)
            {
                if (TransactionByInvestmentId.TryGetValue(investment.InvestmentId, out var transactions))
                    investment.Transactions = transactions.Where(transation => transation.Date < valuationDate).ToList();
                else
                    investment.Transactions = new List<Transaction>();
            }


            //_filesReader.ReadTransactions(investments, valuationDate);

            return investments;
        }

        public Dictionary<string, List<Investment>> GetFondsInvestments(string ownerId, DateTime valuationDate)
        {
            //unique id

            //List<Investment> investments = _filesReader.ReadInvestmentByInvestor(ownerId);
            if (!InvestmetnByOwnerId.TryGetValue(ownerId, out var investments))
            {
                Console.WriteLine("Investments not found for this investor");
                return new Dictionary<string, List<Investment>>();
            }

            var ownerFond = new HashSet<string>(investments
                .Where(investment => investment.InvestmentType.Equals("Fonds"))
                .Select(investment => investment.FondsInvestor));


           
            List<Investment> fondTotalInvestment  = new List<Investment>();

            foreach( string fond in ownerFond)
            {
                if (InvestmetnByOwnerId.TryGetValue(fond, out var fondInvestments))
                {
                    fondTotalInvestment.AddRange(fondInvestments);
                }
            }


            var dictionary = fondTotalInvestment
                .GroupBy(investment => investment.InvestorId)
                .ToDictionary(group => group.Key, group => group.ToList()); 
          

            foreach (var kvp in dictionary)
            {
                var fondInvestments = kvp.Value;


                foreach (var investment in fondInvestments)
                {
                    if (TransactionByInvestmentId.TryGetValue(investment.InvestmentId, out var transactions))
                        investment.Transactions = transactions.Where(transaction => transaction.Date < valuationDate).ToList();
                    else
                        investment.Transactions = new List<Transaction>();
                }
            }


            return dictionary;
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
                .Where(investment => investment.InvestmentType.Equals("Stock"))
                .ToList();

            //List<Quote> quotes = _filesReader.ReadQuotes(stockInvestments, valuationDate);
            //var quotesByIsin = Quotes
            //    .GroupBy(quote => quote.ISIN)
            //    .ToDictionary(group => group.Key, group => group.OrderByDescending(quote => quote.Date).ToList());  


            double stockSumup = 0;

            foreach (var investment in stockInvestments)
            {
                double totalShares = investment.Transactions.Sum(transaction => transaction.Value);

                if (!QuotesByIsin.TryGetValue(investment.ISIN, out var listOfIsin))
                    continue;

                listOfIsin = listOfIsin.Where(isin => isin.Date < valuationDate).ToList();

                var quoteToday = listOfIsin.FirstOrDefault(quote => quote.Date <= valuationDate);

                //if valuationDate is too small
                if (quoteToday == null)
                    quoteToday = listOfIsin.LastOrDefault();


                var marketValue = totalShares * quoteToday.PricePerShare;
                stockSumup += marketValue;

            }

            return stockSumup;
        }


        //PENSAR EM JA PASSAR A LISTA DE INVESTIMENTOS POR PARAMETRO PARA FondEngine tbm !!
        public double FondEngine(string ownerId, DateTime valuationDate)
        {
            double fondSumup = 0;

            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType == "Fonds")
                .ToList();



            //filesReader.ReadTransactions(fonds, valuationDate);

            //old code
            //List<Investment> allInvestments = _filesReader.ReadInvestments();


            //new code
            DateTime t1 = DateTime.Now;
            //Dictionary<string, List<Investment>> dictionary = _filesReader.GetDictionary(ownerId, valuationDate);
            Dictionary<string, List<Investment>> dictionary = GetFondsInvestments(ownerId, valuationDate);

            DateTime t2 = DateTime.Now;
            Console.WriteLine($"return Dictionary: {t2 - t1}");

            foreach (var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum(t => t.Value);

                //old code
                //List<Investment> investmentsOfFound = allInvestments.Where(i => i.InvestorId == fond.FondsInvestor).ToList();
                //_filesReader.ReadTransactions(investmentsOfFound, valuationDate);

                //new code
                dictionary.TryGetValue(fond.FondsInvestor, out var investmentsOfFound);


                //DateTime t5 = DateTime.Now;
                double realStateSumup = RealStateEngine(investmentsOfFound);
                //DateTime t6 = DateTime.Now;
                //Console.WriteLine($"real state engine: {t6 - t5}");

                //DateTime t7 = DateTime.Now;
                double stockSumup = StockEngine(investmentsOfFound, valuationDate);
                //DateTime t8 = DateTime.Now;
                //Console.WriteLine($"stockEngine {t8 - t7}");


                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }

            Console.WriteLine("total asset value of fonds: " + fondSumup);

            return fondSumup;
        }



    }
}
