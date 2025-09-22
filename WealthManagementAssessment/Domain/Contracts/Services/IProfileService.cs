using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public interface IProfileService
    {
        decimal ProfileEngine(string ownerId);

    }
}
