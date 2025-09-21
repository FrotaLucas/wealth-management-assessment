using WealthManagementAssessment.Domain.Contracts.Repository;

namespace WealthManagementAssessment.Domain.Contracts
{
    public class ProfileService
    {

        private readonly IPortfolioRepository _portfolioRepository;

        public ProfileService(IPortfolioRepository portfolioRepository)
        {
            portfolioRepository = portfolioRepository;
        }


        
    }
}
