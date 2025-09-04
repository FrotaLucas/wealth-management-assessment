namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetManagement
    {
        void GetTotalAsset(string ownerId, DateTime valuationDate);

        void GetRealEstateAsset(string ownerId, DateTime valuationDate);

        void GetStockAsset(string ownerId, DateTime valuationDate);

        void GetFundAsset(string ownerId, DateTime valuationDate);
    }
}
