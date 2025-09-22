using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly IProfileService _profileService;

        public ProfileManagementService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public string GetProfile(string ownerId)
        {
            throw new NotImplementedException();
        }
    }
}
