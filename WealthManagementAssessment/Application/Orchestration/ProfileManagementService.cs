using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly IProfileService _profileService;

        public ProfileManagementService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public InvestorProfileEnum GetRiskProfile(string ownerId)
        {

            decimal riskProfile =  _profileService.ProfileEngine(ownerId);

            if (riskProfile == 0)
                return InvestorProfileEnum.Unknown;

            if (riskProfile < 1.33m)
                return InvestorProfileEnum.Conservative;
            else if (riskProfile > 1.33m && riskProfile < 1.66m)
                return InvestorProfileEnum.Moderate;
            else
                return InvestorProfileEnum.Aggressive;
        }
    }
}
