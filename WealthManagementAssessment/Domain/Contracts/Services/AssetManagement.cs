using WealthManagementAssessment.Domain.Contracts.Interfaces;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class AssetManagement : IAssetManagement
    {

        private readonly IAssetRepository _assetRepository;

        public AssetManagement (IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }



        public void DisplayAsset()
        {
            _assetRepository.AssetEngine();

        }
    }

}
