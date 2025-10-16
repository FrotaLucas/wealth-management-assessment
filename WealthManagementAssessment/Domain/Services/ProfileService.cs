using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IFondService _fondService;

        private readonly IRealEstateService _realEstateService;

        private readonly IStockService _stockService;

        public ProfileService(IPortfolioRepository portfolioRepository,
            IFondService fondService, 
            IRealEstateService realEstateService, 
            IStockService stockService)
        {
            _portfolioRepository = portfolioRepository;
            _fondService = fondService;
            _realEstateService = realEstateService;
            _stockService = stockService;
        }

        public decimal ProfileEngine(string ownerId)
        {
            DateTime date = DateTime.Today;

            decimal investmentFond = _fondService.FondEngine(ownerId, date);
            decimal investmentStock = _stockService.StockEngine(ownerId, date);
            decimal investmentRealEstate = _realEstateService.RealEstateEngine(ownerId, date);

            decimal totalWallet = investmentFond + investmentStock + investmentRealEstate;


            List<Fond> fonds = _portfolioRepository.GetFondsByInvestor(ownerId, date)
                .ToList();

            if (totalWallet == 0)
                return 0;

            if (fonds.Any())
            {
                decimal riskFond = CalculateAvareFondRisk(fonds);

                decimal riskProfile = (investmentRealEstate * 1 + investmentStock * 2 + investmentFond * riskFond) / totalWallet;

                return riskProfile;
            }

            return (investmentRealEstate * 1 + investmentStock * 2 ) / totalWallet; ;
        }

        private decimal CalculateAvareFondRisk(List<Fond> fonds)
        {
            DateTime date = DateTime.Today; 
            decimal totalRisk = 0;

            foreach (var fond in fonds)
            {
                decimal investmentRealEstate = _realEstateService.RealEstateEngine(fond.FondsInvestor, date);
                decimal investmentStock = _stockService.StockEngine(fond.FondsInvestor, date);
                decimal totalInvestments = investmentStock + investmentRealEstate;

                totalRisk = totalRisk + (investmentRealEstate * 1 + investmentStock * 2) / totalInvestments;
            }

            decimal fundRisk = totalRisk / fonds.Count;

            return fundRisk;
        }
    }
}