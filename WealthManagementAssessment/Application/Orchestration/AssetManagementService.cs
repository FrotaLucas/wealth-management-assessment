using Microsoft.Extensions.Options;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Application.Models;
using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Application.Orchestration
{
    public class AssetManagementService : IAssetManagementService
    {

        private readonly IEnumerable<IAssetTypeService> _assetTypesService;

        private readonly IProfileService _profileService;

        private readonly AppConfig _appConfig;
        public AssetManagementService(
                                      IProfileService profileService,
                                      IOptions<AppConfig> appConfig,
                                      IEnumerable<IAssetTypeService> assetTypesService)
        {
            _profileService = profileService;
            _appConfig = appConfig.Value;
            _assetTypesService = assetTypesService;
        }

        public InvestorBalanceResult GetFondAsset(string ownerId, DateTime valuationDate)
        {
            var fondService = _assetTypesService.FirstOrDefault( s=> s.AssetType.Equals(AssetTypeServiceEnum.Fond) );

            decimal fond = fondService.CalculateBalance(ownerId,valuationDate);

            var result = new InvestorBalanceResult { FondBalance = fond };  

            return result;
        }

        public InvestorBalanceResult GetRealEstateAsset(string ownerId, DateTime valuationDate)
        {
            var realEstateService = _assetTypesService.FirstOrDefault(s => s.AssetType.Equals(AssetTypeServiceEnum.RealEstate) );

            decimal realEstate = realEstateService.CalculateBalance(ownerId, valuationDate);

            var result =  new InvestorBalanceResult { RealEstateBalance = realEstate };

            return result;
        }

        public InvestorBalanceResult GetStockAsset(string ownerId, DateTime valuationDate)
        {
            var stockService = _assetTypesService.FirstOrDefault(s => s.AssetType.Equals (AssetTypeServiceEnum.Stock) );    

            decimal stock = stockService.CalculateBalance(ownerId, valuationDate) ;

            var result = new InvestorBalanceResult { StockBalance = stock };

            return result;
        }

        public InvestorBalanceResult GetTotalAsset(string ownerId, DateTime valuationDate)
        {
            var balances = _assetTypesService.ToDictionary(typeService => typeService.AssetType, typeService => typeService.CalculateBalance(ownerId, valuationDate));

            var result = new InvestorBalanceResult
            {

                RealEstateBalance = balances.GetValueOrDefault(AssetTypeServiceEnum.RealEstate),
                StockBalance = balances.GetValueOrDefault(AssetTypeServiceEnum.Stock),
                FondBalance = balances.GetValueOrDefault(AssetTypeServiceEnum.Fond)
            };

            return result;
        }


        public InvestorProfileEnum GetRiskProfile(string ownerId)
        {
            decimal ConservativeUpperLimit = _appConfig.RiskProfile.ConservativeUpperLimit;

            decimal ModerateUpperLimit = _appConfig.RiskProfile.ModerateUpperLimit;

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
