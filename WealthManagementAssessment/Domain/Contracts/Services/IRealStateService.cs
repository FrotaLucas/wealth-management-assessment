namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealStateService
    {
        decimal RealStateEngine(string ownerId, DateTime valuationDate);
    }
}
