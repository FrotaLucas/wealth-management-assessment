using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Domain.Enums;
using WealthManagementAssessment.Infrastructure.DataProvider;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private IReadOnlyDictionary<string, List<InvestmentData>> InvestmentsByOwnerId { get; }

        private IReadOnlyDictionary<string, List<Quote>> QuotesByIsin { get; }

        private IReadOnlyDictionary<string, List<Transaction>> TransactionsByInvestmentId { get; }  

        public PortfolioRepository(IDataSource dataSource)
        {

            QuotesByIsin = dataSource.ReadQuotes();

            TransactionsByInvestmentId = dataSource.ReadTransactions();

            InvestmentsByOwnerId = dataSource.ReadInvestments();

        }

        private void LoadTransactions<T>(IEnumerable<T> investments, DateTime valuationDate) where T : Investment
        {
            foreach (var investment in investments)
            {

                if (TransactionsByInvestmentId.TryGetValue(investment.InvestmentId, out var transactions))
                    investment.Transactions = transactions.Where(transation => transation.Date <= valuationDate).ToList();
                else
                    investment.Transactions = new List<Transaction>();
            }
        }

        public List<Stock> GetStocksByInvestor(string ownerId, DateTime valuationDate)
        {
            if (!InvestmentsByOwnerId.TryGetValue(ownerId, out var investmentData))
                return new List<Stock>();

            List<Stock> stocks = investmentData
                .Where(investment => investment.InvestmentType.Equals(InvestmentTypeEnum.Stock))
                .Select(investment => new Stock
                {
                    InvestorId = investment.InvestorId,
                    InvestmentId = investment.InvestmentId,
                    ISIN = investment.ISIN,
                })
                .ToList();

            LoadTransactions(stocks, valuationDate);

            return stocks;
        }

        public List<RealEstate> GetRealEstatesByInvestor(string ownerId, DateTime valuationDate)
        {
            if(!InvestmentsByOwnerId.TryGetValue(ownerId, out var investmentData))
                return new List<RealEstate>();

            List<RealEstate> realEstates = investmentData
                .Where(investment => investment.InvestmentType.Equals(InvestmentTypeEnum.RealEstate))
                .Select(investment => new RealEstate
                {
                    InvestorId = investment.InvestorId,
                    InvestmentId = investment.InvestmentId,
                    City = investment.City,
                })
                .ToList();

            LoadTransactions(realEstates, valuationDate);
          
            return realEstates;
        }

        public List<Fond> GetFondsByInvestor(string ownerId, DateTime valuationDate)
        {
            if (!InvestmentsByOwnerId.TryGetValue(ownerId, out var investmentData))
                return new List<Fond>();

            List<Fond> fonds = investmentData
                .Where(investment => investment.InvestmentType.Equals(InvestmentTypeEnum.Fonds))
                .Select(investment => new Fond
                {
                    InvestorId = investment.InvestorId,
                    InvestmentId = investment.InvestmentId,
                    FondsInvestor = investment.FondsInvestor,
                })
                .ToList();

            LoadTransactions(fonds, valuationDate);

            return fonds;
        }

        public Quote GetQuoteByDate(string isin, DateTime valuationDate)
        {
            if (!QuotesByIsin.TryGetValue(isin, out var isinQuotes))
                return new Quote();

            
           var quote = isinQuotes
                .Where(quote => quote.Date <= valuationDate)
                .FirstOrDefault();

            if (quote == null) 
                return new Quote();

            return quote;
        }
    }
}