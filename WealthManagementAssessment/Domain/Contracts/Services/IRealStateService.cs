using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealStateService
    {
        decimal RealStateEngine(List<Investment> investments);
    }
}
