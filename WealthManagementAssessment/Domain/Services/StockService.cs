using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;

namespace WealthManagementAssessment.Domain.Services
{
    public class StockService : IStockService
    {

        private readonly IPortfolioRepository _assetRepository;

        public StockService(IPortfolioRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public decimal StockEngine(string ownerId, DateTime valuationDate)
        {

            var stockInvestments = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType.Equals("Stock"))
                .ToList();

            decimal stockSumup = 0;

            foreach (var investment in stockInvestments)
            {
                decimal totalShares = investment.Transactions.Sum(transaction => transaction.Value);

                
                if (!_assetRepository.QuotesByIsin.TryGetValue(investment.ISIN, out var isinQuotes))
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


    }

}
