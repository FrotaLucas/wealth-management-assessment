using WealthManagementAssessment.Domain.Contracts.Interfaces;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class AssetManagement
    {

        private readonly IAssetValuation _assetValuation;

        public AssetManagement (IAssetValuation assetValuation)
        {
            _assetValuation = assetValuation;
        }



        public void DisplayAsset()
        {

        }
    }

}
