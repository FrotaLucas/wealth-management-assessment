namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealEstateService
    {
        decimal RealEstateEngine(string ownerId, DateTime valuationDate);
    }
}