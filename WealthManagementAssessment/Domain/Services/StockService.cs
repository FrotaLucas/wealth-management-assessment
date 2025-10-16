using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Domain.Services
{
    public class StockService : IStockService, IAssetTypeService
    {

        private readonly IPortfolioRepository _portfolioRepository;

        public StockService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public AssetTypeServiceEnum AssetType => AssetTypeServiceEnum.Stock;
      

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
        public decimal CalculateBalance(string ownerId, DateTime valuationDate) => StockEngine(ownerId, valuationDate);
    }
}