namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFondService
    {
        double FondEngine(string ownerId, DateTime valuationDate);
    }
}
