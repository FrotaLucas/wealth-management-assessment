namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealEstateService
    {
        decimal RealEstate(string ownerId, DateTime valuationDate);
    }
}