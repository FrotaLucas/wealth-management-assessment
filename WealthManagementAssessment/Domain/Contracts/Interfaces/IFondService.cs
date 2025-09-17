namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFondService
    {
        decimal FondEngine(string ownerId, DateTime valuationDate);
    }
}
