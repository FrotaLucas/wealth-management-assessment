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
            //only realstate
            var investments = _assetRepository.GetAllInvestments(ownerId, valuationDate);

            _assetRepository.RealStateEngine(investments);


            //fonds
            //_assetRepository.LoadFilesJustOnce(ownerId, valuationDate);

            //_assetRepository.FondEngine(valuationDate);

        }

        //   public void DisplayRealState

        //    public void DisplayStocks

        //   public void DisplayFonds
    }

}
