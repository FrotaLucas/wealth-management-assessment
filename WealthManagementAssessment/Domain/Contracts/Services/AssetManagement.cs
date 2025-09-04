using WealthManagementAssessment.Domain.Contracts.Interfaces;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class AssetManagement : IAssetManagement
    {

        private readonly IAssetRepository _assetRepository;

        public AssetManagement(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public void GetFundAsset(string ownerId, DateTime valuationDate)
        {
            double asset = _assetRepository.FondEngine(ownerId, valuationDate);

            Console.WriteLine($"Your wallet in Funds represent: {asset} Euros.");
        }

        public void GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            throw new NotImplementedException();
        }

        public void GetStockAsset(string ownerId, DateTime valuationDate)
        {
            throw new NotImplementedException();
        }

        public void GetTotalAsset(string ownerId, DateTime valuationDate)
        {
            //only realstate
            var investments = _assetRepository.GetAllInvestments(ownerId, valuationDate);

            //_assetRepository.RealStateEngine(investments);


            //fonds
            //_assetRepository.LoadFilesJustOnce(ownerId, valuationDate);

            //_assetRepository.FondEngine(ownerId, valuationDate);

            _assetRepository.StockEngine(investments, valuationDate);

        }

        //   public void DisplayRealState

        //    public void DisplayStocks

        //   public void DisplayFonds
    }

}
