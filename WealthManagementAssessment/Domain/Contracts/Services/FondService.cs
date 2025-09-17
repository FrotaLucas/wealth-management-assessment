using WealthManagementAssessment.Domain.Contracts.Interfaces;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class FondService : IFondService
    {
        private readonly IAssetRepository _assetRepository;

        public FondService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
    }
}
