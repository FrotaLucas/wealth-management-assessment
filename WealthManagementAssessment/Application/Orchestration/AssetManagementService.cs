using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class AssetManagementService : IAssetManagement
    {
        private readonly IPortfolioService _portifolioService;

        private readonly IAssetRepository _assetRepository;


        public AssetManagementService(IAssetRepository assetRepository, IPortfolioService portifolioService)
        {
            _assetRepository = assetRepository;
            _portifolioService = portifolioService;
        }

        public void GetFundAsset(string ownerId, DateTime valuationDate)
        {
            //double asset = _assetRepository.FondEngine(ownerId, valuationDate);

            //Console.WriteLine($"Your Fund wallet is : {asset} Euros.");
        }

        public void GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            double asset = _assetRepository.RealStateEngine(investments);

            Console.WriteLine($"Your Real Estate wallet is : {asset} Euros.");

        }

        public void GetStockAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            double asset = _assetRepository.StockEngine(investments, valuationDate);

            Console.WriteLine($"Your Stock wallet is : {asset} Euros.");
        }

        
        
        //chamar de balance
        public void GetTotalAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            double realEstateAsset = _assetRepository.RealStateEngine(investments);

            double stockAsset = _assetRepository.StockEngine(investments, valuationDate);


            //double fundAsset = _assetRepository.FondEngine(ownerId, valuationDate);

            //Console.WriteLine($"Your total wallet is : {realEstateAsset + stockAsset + fundAsset} Euros.");


        }

    }

}
