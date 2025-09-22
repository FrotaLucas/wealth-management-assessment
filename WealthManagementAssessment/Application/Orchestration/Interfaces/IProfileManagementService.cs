using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IProfileManagementService
    {
        public string GetProfile(string ownerId);
    }
}
