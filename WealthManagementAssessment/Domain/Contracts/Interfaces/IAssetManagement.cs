namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetManagement
    {
        void GetTotalAsset(string ownerId, DateTime valuationDate);
    }
}
