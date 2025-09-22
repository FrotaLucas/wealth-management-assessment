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

            decimal riskProfile =  _profileService.ProfileEngine(ownerId);

            if (riskProfile < 1.33m)
                return "conservative";
            else if (riskProfile > 1.33m && riskProfile < 1.66m)
                return "moderate";
            else
                return "aggressive";
        }
    }
}
