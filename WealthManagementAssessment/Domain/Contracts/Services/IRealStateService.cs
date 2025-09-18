using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealStateService
    {
        decimal RealStateEngine(string ownerId, DateTime valuationDate);
    }
}
