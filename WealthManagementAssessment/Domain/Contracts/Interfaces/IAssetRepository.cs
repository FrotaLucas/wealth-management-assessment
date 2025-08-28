using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetRepository
    {

        void FilesReader();


        double RealStateEngine(List<Investment> investments);

        double StockEngine(List<Investment> investments);

        double FondEngine();

        void AssetEngine();
    }
}
