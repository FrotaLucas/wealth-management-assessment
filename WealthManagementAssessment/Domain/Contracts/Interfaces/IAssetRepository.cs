using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetRepository
    {
        double RealStateEngine(List<Investment> investments);

        double StockEngine(List<Investment> investments, DateTime valuationDate);

        double FondEngine(DateTime valuationDate);

        void LoadFiles(string ownerId, DateTime valuationDate);

    }
}
