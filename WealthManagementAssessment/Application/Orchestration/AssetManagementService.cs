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
            decimal asset = _fondService.FondEngine(ownerId, valuationDate);

            Console.WriteLine($"Your Fund wallet is : {asset:N2} Euros.\n");
        }

        public void GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            //List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            decimal asset = _realStateService.RealStateEngine(ownerId, valuationDate);

            Console.WriteLine($"Your Real Estate wallet is : {asset:N2} Euros.\n");

        }

        public void GetStockAsset(string ownerId, DateTime valuationDate)
        {
            //List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            decimal asset = _stockService.StockEngine(ownerId, valuationDate);

            Console.WriteLine($"Your Stock wallet is : {asset:N2} Euros.\n");
        }

        
        
        //chamar de balance
        public void GetTotalAsset(string ownerId, DateTime valuationDate)
        {
            List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            decimal realEstateAsset = _realStateService.RealStateEngine(ownerId, valuationDate);

            decimal stockAsset = _stockService.StockEngine(ownerId, valuationDate);

            decimal fundAsset = _fondService.FondEngine(ownerId, valuationDate);

            Console.WriteLine($"Your total wallet is : {(realEstateAsset + stockAsset + fundAsset):N2} Euros.\n");

        }

    }

}
