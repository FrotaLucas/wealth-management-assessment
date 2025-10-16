namespace WealthManagementAssessment.Domain.Enums
{
    public interface IAssetTypeService
    {
        //enum type service
        AssetTypeServiceEnum AssetType { get; }
        public decimal CalculateBalance(string ownerId, DateTime valuationDate);
    }
}
