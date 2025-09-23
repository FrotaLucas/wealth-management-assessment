using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class ProfileManagementService : IProfileManagementService
    {

        private const decimal ConservativeUpperLimit = 1.33m;

        private const decimal ModerateUpperLimit = 1.66m;


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

            if (riskProfile < ConservativeUpperLimit)
                return InvestorProfileEnum.Conservative;

            else if (riskProfile > ConservativeUpperLimit && riskProfile < ModerateUpperLimit)
                return InvestorProfileEnum.Moderate;

            else
                return InvestorProfileEnum.Aggressive;
        }
    }
}
