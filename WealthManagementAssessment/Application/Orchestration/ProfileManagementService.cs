using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly IPortfolioRepository _assetRepository;

        public ProfileManagementService(IPortfolioRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            return investments;
        }
    }
}
