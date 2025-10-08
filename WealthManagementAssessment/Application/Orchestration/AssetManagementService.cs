using WealthManagementAssessment.Application.Models;
using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class AssetManagementService : IAssetManagementService
    {

        private readonly IStockService _stockService;

        private readonly IRealEstateService _realEstateService;

        private readonly IFondService _fondService;

        private readonly IProfileService _profileService;

        public AssetManagementService(IStockService stockService,
                                      IRealEstateService realStateService,
                                      IFondService fondService,
                                      IProfileService profileService)
        {
            _stockService = stockService;
            _realEstateService = realStateService;
            _fondService = fondService;
            _profileService = profileService;
        }

        public InvestorBalanceResult GetFondAsset(string ownerId, DateTime valuationDate)
        {
            decimal fond = _fondService.FondEngine(ownerId, valuationDate);

            var result = new InvestorBalanceResult { FondBalance = fond };  

            return result;
        }

        public InvestorBalanceResult GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            decimal realEstate = _realEstateService.RealStateEngine(ownerId, valuationDate);

            var result =  new InvestorBalanceResult { RealStateBalance = realEstate };

            return result;
        }

        public InvestorBalanceResult GetStockAsset(string ownerId, DateTime valuationDate)
        {
            decimal stock = _stockService.StockEngine(ownerId, valuationDate);

            var result = new InvestorBalanceResult { StockBalance = stock };

            return result;
        }

        public InvestorBalanceResult GetTotalAsset(string ownerId, DateTime valuationDate)
        {
            decimal realEstateAsset = _realEstateService.RealStateEngine(ownerId, valuationDate);

            decimal stockAsset = _stockService.StockEngine(ownerId, valuationDate);

            decimal fondAsset = _fondService.FondEngine(ownerId, valuationDate);

            var result = new InvestorBalanceResult
            {
                RealStateBalance = realEstateAsset,
                StockBalance = stockAsset,
                FondBalance = fondAsset
            };

            return result;
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
