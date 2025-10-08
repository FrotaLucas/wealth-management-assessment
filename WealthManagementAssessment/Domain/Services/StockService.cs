using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class StockService : IStockService
    {

        private readonly IPortfolioRepository _portfolioRepository;

        public StockService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public decimal StockEngine(string ownerId, DateTime valuationDate)
        {

            List<Stock> stocks = _portfolioRepository.GetStocksByInvestor(ownerId, valuationDate)
                .ToList();

            decimal stockSumup = 0;

            foreach (var stock in stocks)
            {
                decimal totalShares = stock.Transactions.Sum(transaction => transaction.Value);

                var quoteToday = _portfolioRepository.GetQuoteByDate(stock.ISIN, valuationDate);

                if(quoteToday == null || quoteToday.PricePerShare == 0 )
                {
                    continue;
                }

                var marketValue = totalShares * quoteToday.PricePerShare;
                stockSumup += marketValue;
            }

            return stockSumup;
        }
    }
}