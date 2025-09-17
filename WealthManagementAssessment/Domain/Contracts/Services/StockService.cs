using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class StockService : IStockService
    {

        private readonly IAssetRepository _assetRepository;

        public StockService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public double StockEngine(List<Investment> investments, DateTime valuationDate)
        {

            var stockInvestments = investments
                .Where(investment => investment.InvestmentType.Equals("Stock"))
                .ToList();


            double stockSumup = 0;

            foreach (var investment in stockInvestments)
            {
                double totalShares = investment.Transactions.Sum(transaction => transaction.Value);

                
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
