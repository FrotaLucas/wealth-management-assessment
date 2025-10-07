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

                //if (!_portfolioRepository.QuotesByIsin.TryGetValue(stock.ISIN, out var isinQuotes))
                //    continue;


                //var quoteToday = isinQuotes.FirstOrDefault(quote => quote.Date <= valuationDate);

                //if (quoteToday == null)
                //    quoteToday = isinQuotes.LastOrDefault();

                var marketValue = totalShares * quoteToday.PricePerShare;
                stockSumup += marketValue;
            }

            return stockSumup;
        }
    }
}