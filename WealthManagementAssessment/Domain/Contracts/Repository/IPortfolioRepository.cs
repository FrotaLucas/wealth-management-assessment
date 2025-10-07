using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Infrastructure.DataProvider;

namespace WealthManagementAssessment.Domain.Contracts.Repository
{
    public interface IPortfolioRepository
    {
        List<Stock> GetStocksByInvestor(string ownerId, DateTime valuationDate);

        List<RealEstate> GetRealEstatesByInvestor(string ownerId, DateTime valuationDate);

        List<Fond> GetFondsByInvestor(string ownerId, DateTime valuationDate);

        Quote GetQuoteByDate(string isin,  DateTime valuationDate);
    }
}
