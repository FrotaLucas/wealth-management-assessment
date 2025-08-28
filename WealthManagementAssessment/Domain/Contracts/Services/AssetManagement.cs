using WealthManagementAssessment.Domain.Contracts.Interfaces;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class AssetManagement
    {

        private readonly IAssetRepository _assetValuation;

        public AssetManagement (IAssetRepository assetValuation)
        {
            _assetValuation = assetValuation;
        }



        public void DisplayAsset()
        {

        }
    }

}
