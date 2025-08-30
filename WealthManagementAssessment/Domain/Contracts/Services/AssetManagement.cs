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


        public void DisplayAsset(string ownerId, DateTime valuationDate)
        {
            //_assetRepository.LoadFilesJustOnce(ownerId, valuationDate);

            var investments = _assetRepository.GetAllInvestments(ownerId, valuationDate);

            _assetRepository.RealStateEngine(investments);

            //_assetRepository.FondEngine(valuationDate);

        }
    }

}
