using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IProfileManagementService
    {
        public InvestorProfileEnum GetRiskProfile(string ownerId);
    }
}
