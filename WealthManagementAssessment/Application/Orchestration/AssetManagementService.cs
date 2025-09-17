using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class AssetManagementService : IAssetManagement
    {
        private readonly IPortfolioService _portfolioService;

        private readonly IStockService _stockService;

        private readonly IRealStateService _realStateService;

        private readonly IFondService _fondService;

        //PENSAR NUNA FORMA DE REDUZIR ESSE TAMANHO DOS PARAMETROS DO CONSTRUTOR!!
        public AssetManagementService(IPortfolioService portfolioService, IStockService stockService, IRealStateService realStateService, IFondService fondService)
        {
            _portfolioService = portfolioService;
            _stockService = stockService;
            _realStateService = realStateService;
            _fondService = fondService;
        }

        public void GetFundAsset(string ownerId, DateTime valuationDate)
        {
            double asset = _fondService.FondEngine(ownerId, valuationDate);

            Console.WriteLine($"Your Fund wallet is : {asset} Euros.");
        }

        public void GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            double asset = _realStateService.RealStateEngine(investments);

            Console.WriteLine($"Your Real Estate wallet is : {asset} Euros.");

        }

        public void GetStockAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            double asset = _stockService.StockEngine(investments, valuationDate);

            Console.WriteLine($"Your Stock wallet is : {asset} Euros.");
        }

        
        
        //chamar de balance
        public void GetTotalAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            //double realEstateAsset = _assetRepository.RealStateEngine(investments);

            //double stockAsset = _assetRepository.StockEngine(investments, valuationDate);


            //double fundAsset = _assetRepository.FondEngine(ownerId, valuationDate);

            //Console.WriteLine($"Your total wallet is : {realEstateAsset + stockAsset + fundAsset} Euros.");


        }

    }

}
