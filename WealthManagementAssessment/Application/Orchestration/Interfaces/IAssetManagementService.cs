using WealthManagementAssessment.Application.Models;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IAssetManagementService
    {
        decimal GetTotalAsset(string ownerId, DateTime valuationDate);

        InvestorBalanceResult GetRealEstateAsset(string ownerId, DateTime valuationDate);

        decimal GetStockAsset(string ownerId, DateTime valuationDate);

        decimal GetFundAsset(string ownerId, DateTime valuationDate);

        InvestorProfileEnum GetRiskProfile(string onwerId);
    }
}
