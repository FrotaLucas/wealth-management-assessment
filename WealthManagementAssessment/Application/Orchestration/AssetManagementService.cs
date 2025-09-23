using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class AssetManagementService : IAssetManagementService
    {

        private readonly IStockService _stockService;

        private readonly IRealStateService _realStateService;

        private readonly IFondService _fondService;

        private readonly IProfileService _profileService;

        //PENSAR NUNA FORMA DE REDUZIR ESSE TAMANHO DOS PARAMETROS DO CONSTRUTOR!!
        public AssetManagementService(IStockService stockService, IRealStateService realStateService, IFondService fondService, IProfileService profileService)
        {
            _stockService = stockService;
            _realStateService = realStateService;
            _fondService = fondService;
            _profileService = profileService;
        }

        public decimal GetFundAsset(string ownerId, DateTime valuationDate)
        {
            decimal asset = _fondService.FondEngine(ownerId, valuationDate);

            return asset;
        }

        public decimal GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            decimal asset = _realStateService.RealStateEngine(ownerId, valuationDate);

            return asset;
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
            //List<Investment> investments = _portfolioService.GetAllInvestmentsByInvestor(ownerId, valuationDate);

            decimal realEstateAsset = _realStateService.RealStateEngine(ownerId, valuationDate);

            decimal stockAsset = _stockService.StockEngine(ownerId, valuationDate);

            decimal fundAsset = _fondService.FondEngine(ownerId, valuationDate);

            Console.WriteLine($"Your total wallet is : {(realEstateAsset + stockAsset + fundAsset):N2} Euros.\n");

        }


        public InvestorProfileEnum GetRiskProfile(string ownerId)
        {
            //ENCONTRAR UMA FORMA DE DEFINIR UNIVERSALMENTE ESSES LIMITES
            decimal ConservativeUpperLimit = 1.33m;

            decimal ModerateUpperLimit = 1.66m;


            decimal riskProfile = _profileService.ProfileEngine(ownerId);

            if (riskProfile == 0)
                return InvestorProfileEnum.Unknown;

            if (riskProfile < ConservativeUpperLimit)
                return InvestorProfileEnum.Conservative;

            else if (riskProfile > ConservativeUpperLimit && riskProfile < ModerateUpperLimit)
                return InvestorProfileEnum.Moderate;

            else
                return InvestorProfileEnum.Aggressive;
        }

    }

}
