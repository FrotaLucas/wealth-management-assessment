using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {

        //NAO VOU PRECISAR DESSA PROPRIEDADE E SO BASTA O PARAMETRO filesReader no CTOR MAS PQ ?
        private readonly IFilesReader _filesReader;

        public Dictionary<string, List<Investment>> InvestmentsByOwnerId;

        public Dictionary<string, List<Quote>> QuotesByIsin;

        public Dictionary<string, List<Transaction>> TransactionsByInvestmentId;

        public AssetRepository(IFilesReader filesReader)
        {
            _filesReader = filesReader;

            QuotesByIsin = _filesReader.ReadQuotes();

            TransactionsByInvestmentId = _filesReader.ReadTransactions();
            
        }

        public List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _filesReader.ReadInvestmentByInvestor(ownerId);

            foreach(var investment in investments)
            {

                if (TransactionsByInvestmentId.TryGetValue(investment.InvestmentId, out var transactions))
                    investment.Transactions = transactions.Where(transation => transation.Date <= valuationDate).ToList();
                else
                    investment.Transactions = new List<Transaction>();
            }

        

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

            var stockInvestments = investments
                .Where( investment => investment.InvestmentType.Equals("Stock") )
                .ToList();
            

            double stockSumup = 0;

            foreach(var investment in stockInvestments)
            {
                double totalShares = investment.Transactions.Sum(transaction => transaction.Value);

                if (!QuotesByIsin.TryGetValue(investment.ISIN, out var isinQuotes))
                    continue;

                var quoteToday = isinQuotes.FirstOrDefault(quote => quote.Date <= valuationDate);

                //if valuationDate is too small
                if (quoteToday == null)
                    quoteToday = isinQuotes.LastOrDefault();

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

            //_filesReader.ReadTransactions(fonds, valuationDate);

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

            return fondSumup;
        }
    }
}
