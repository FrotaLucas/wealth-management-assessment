using WealthManagementAssessment.Domain.Contracts.Repository;

namespace WealthManagementAssessment.Domain.Contracts
{
    public class ProfileService
    {

        private readonly IPortfolioRepository portfolioRepository;

        public ProfileService(IPortfolioRepository portfolioRepository)
        {
            this.portfolioRepository = portfolioRepository;
        }


        
    }
}
