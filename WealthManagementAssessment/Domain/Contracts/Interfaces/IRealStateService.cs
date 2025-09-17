using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IRealStateService
    {
        double RealStateEngine(List<Investment> investments);
    }
}
