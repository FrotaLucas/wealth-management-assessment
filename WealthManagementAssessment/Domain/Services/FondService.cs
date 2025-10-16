using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class FondService : IFondService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IRealEstateService _realEstateService;

        private readonly IStockService _stockService;
        
        public FondService(IPortfolioRepository portfolioRepository, IRealEstateService realEstateService, IStockService stockService)
        {
            _portfolioRepository = portfolioRepository;
            _realEstateService = realEstateService;
            _stockService = stockService;
        }

        public decimal FondEngine(string ownerId, DateTime valuationDate)
        {
            decimal fondSumup = 0;

            List<Fond> fonds = _portfolioRepository.GetFondsByInvestor(ownerId, valuationDate)
                .ToList();

            foreach (var fond in fonds)
            {
                decimal totalPercentage = fond.Transactions.Sum(t => t.Value);

                decimal realEstateSumup = _realEstateService.RealEstateEngine(fond.FondsInvestor, valuationDate);
                decimal stockSumup = _stockService.StockEngine(fond.FondsInvestor, valuationDate);
                
                fondSumup = fondSumup + totalPercentage * (realEstateSumup + stockSumup);
            }

            return fondSumup;
        }
    }
}