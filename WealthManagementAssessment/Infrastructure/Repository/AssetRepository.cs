using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {

        public IReadOnlyDictionary<string, List<Investment>> InvestmentsByOwnerId { get; }

        public IReadOnlyDictionary<string, List<Quote>> QuotesByIsin { get; }

        public IReadOnlyDictionary<string, List<Transaction>> TransactionsByInvestmentId { get; }  

        public AssetRepository(IFilesReader filesReader)
        {

            QuotesByIsin = filesReader.ReadQuotes();

            TransactionsByInvestmentId = filesReader.ReadTransactions();

            InvestmentsByOwnerId = filesReader.ReadInvestments();

        }

        public List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate)
        {
            if (!InvestmentsByOwnerId.TryGetValue(ownerId, out var investments))
            {
                Console.WriteLine("Investor does not have investments.");
                return new List<Investment>();
            }

            foreach (var investment in investments)
            {

                if (TransactionsByInvestmentId.TryGetValue(investment.InvestmentId, out var transactions))
                    investment.Transactions = transactions.Where(transation => transation.Date <= valuationDate).ToList();
                else
                    investment.Transactions = new List<Transaction>();
            }

            return investments;
        }

        public Dictionary<string, List<Investment>> GetAllInvestmentsByFonds(string ownerId, DateTime valuationDate)
        {

            //TALVEZ NAO PRECISE DESSA LINHA 
            if (!InvestmentsByOwnerId.TryGetValue(ownerId, out var fonds))
            {
                Console.WriteLine("Investments not found for this investor");
                return new Dictionary<string, List<Investment>>();
            }


            var fondList = new HashSet<string>(fonds
                .Where(investment => investment.InvestmentType.Equals("Fonds"))
                .Select(investment => investment.FondsInvestor));


            List<Investment> fondInvestment = new List<Investment>();
            foreach (string fond in fondList)
            {
                if (InvestmentsByOwnerId.TryGetValue(fond, out var investments))
                    fondInvestment.AddRange(investments);
            }

            var dictionary = fondInvestment
                .GroupBy(investment => investment.InvestorId)
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach (var kvp in dictionary)
            {

                var investments = kvp.Value;

                foreach (var investment in investments)
                {
                    if (TransactionsByInvestmentId.TryGetValue(investment.InvestmentId, out var transactions))
                        investment.Transactions = transactions.Where(transaction => transaction.Date < valuationDate).ToList();
                    else
                        investment.Transactions = new List<Transaction>();
                }
            }

            return dictionary;

        }

        //public double RealStateEngine(List<Investment> investments)
        //{

        //    var realStateSumup = investments
        //        .Where(investment => investment.InvestmentType == "RealEstate")
        //        .SelectMany(investment => investment.Transactions)
        //        .Sum(transaction => transaction.Value);

        //    return realStateSumup;
        //}

        //public double StockEngine(List<Investment> investments, DateTime valuationDate)
        //{

        //    var stockInvestments = investments
        //        .Where(investment => investment.InvestmentType.Equals("Stock"))
        //        .ToList();


        //    double stockSumup = 0;

        //    foreach (var investment in stockInvestments)
        //    {
        //        double totalShares = investment.Transactions.Sum(transaction => transaction.Value);

        //        if (!QuotesByIsin.TryGetValue(investment.ISIN, out var isinQuotes))
        //            continue;

        //        var quoteToday = isinQuotes.FirstOrDefault(quote => quote.Date <= valuationDate);

        //        //if valuationDate is too small
        //        if (quoteToday == null)
        //            quoteToday = isinQuotes.LastOrDefault();

        //        var marketValue = totalShares * quoteToday.PricePerShare;
        //        stockSumup += marketValue;

        //    }

        //    return stockSumup;
        //}


        //PENSAR EM JA PASSAR A LISTA DE INVESTIMENTOS POR PARAMETRO PARA FondEngine tbm !!

        //public double FondEngine(string ownerId, DateTime valuationDate)
        //{
        //    double fondSumup = 0;

        //    //USAR ENUM ao inves de STRING!!!!!!!!
        //    List<Investment> fonds = GetAllInvestmentsByInvestor(ownerId, valuationDate)
        //        .Where(investment => investment.InvestmentType == "Fonds")
        //        .ToList();

        //    //_filesReader.ReadTransactions(fonds, valuationDate);

        //    //old code
        //    //List<Investment> allInvestments = _filesReader.ReadInvestments();


        //    //new code
        //    //Dictionary<string, List<Investment>> dictionary = _filesReader.GetDictionary(ownerId, valuationDate);
        //    Dictionary<string, List<Investment>> dictionary = GetAllInvestmentsByFonds(ownerId, valuationDate);



        //    foreach (var fond in fonds)
        //    {
        //        double totalPercentage = fond.Transactions.Sum(t => t.Value);

        //        //old code
        //        //List<Investment> investmentsOfFound = allInvestments.Where(i => i.InvestorId == fond.FondsInvestor).ToList();
        //        //_filesReader.ReadTransactions(investmentsOfFound, valuationDate);

        //        //new code
        //        dictionary.TryGetValue(fond.FondsInvestor, out var investmentsOfFound);


        //        double realStateSumup = RealStateEngine(investmentsOfFound);

        //        double stockSumup = StockEngine(investmentsOfFound, valuationDate);


        //        fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
        //    }

        //    return fondSumup;
        //}

    }
}
