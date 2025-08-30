using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetRepository
    {
        double RealStateEngine(List<Investment> investments);

        double StockEngine(List<Investment> investments, DateTime valuationDate);

        double FondEngine(DateTime valuationDate);

        List<Investment> GetAllInvestments(string ownerId, DateTime valuationDate);

        void LoadFilesJustOnce(string ownerId, DateTime valuationDate);

    }
}
