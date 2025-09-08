using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetRepository
    {
        double RealStateEngine(List<Investment> investments);

        double StockEngine(List<Investment> investments, DateTime valuationDate);

        double FondEngine(string ownerId ,DateTime valuationDate);

        List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate);

    }
}
