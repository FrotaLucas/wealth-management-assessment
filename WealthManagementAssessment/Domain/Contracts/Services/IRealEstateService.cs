namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealEstateService
    {
        decimal RealStateEngine(string ownerId, DateTime valuationDate);
    }
}