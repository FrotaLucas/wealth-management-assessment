using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class PortifolioService : IPortifolioService
    {
        private readonly IAssetRepository _assetRepository;

        public PortifolioService(IAssetRepository assetRepository)
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
