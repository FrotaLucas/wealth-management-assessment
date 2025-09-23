using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IAssetManagementService
    {
        void GetTotalAsset(string ownerId, DateTime valuationDate);

        void GetRealEstateAsset(string ownerId, DateTime valuationDate);

        void GetStockAsset(string ownerId, DateTime valuationDate);

        void GetFundAsset(string ownerId, DateTime valuationDate);

        InvestorProfileEnum GetRiskProfile(string onwerId);
    }
}
