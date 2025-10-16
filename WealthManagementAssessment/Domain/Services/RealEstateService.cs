using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Domain.Services
{
    public class RealEstateService : IRealEstateService, IAssetTypeService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public RealEstateService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public AssetTypeServiceEnum AssetType => AssetTypeServiceEnum.RealEstate;


        public decimal RealEstateEngine(string ownerId, DateTime valuationDate)
        {

            var realEstateSumup = _portfolioRepository.GetRealEstatesByInvestor(ownerId, valuationDate)
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            return realEstateSumup;
        }

        public decimal CalculateBalance(string ownerId, DateTime valuationDate) => RealEstateEngine(ownerId, valuationDate);
      
    }
}