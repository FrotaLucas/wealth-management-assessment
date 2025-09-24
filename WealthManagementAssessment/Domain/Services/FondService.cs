using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Domain.Services
{
    public class FondService : IFondService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IRealStateService _realStateService;

        private readonly IStockService _stockService;
        
        public FondService(IPortfolioRepository portfolioRepository, IRealStateService realStateService, IStockService stockService)
        {
            _portfolioRepository = portfolioRepository;
            _realStateService = realStateService;
            _stockService = stockService;
        }

        public decimal FondEngine(string ownerId, DateTime valuationDate)
        {
            decimal fondSumup = 0;

            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = _portfolioRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType.Equals(InvestmentTypeEnum.Fonds))
                .ToList();

            foreach (var fond in fonds)
            {
                decimal totalPercentage = fond.Transactions.Sum(t => t.Value);

                decimal realStateSumup = _realStateService.RealStateEngine(fond.FondsInvestor, valuationDate);
                decimal stockSumup = _stockService.StockEngine(fond.FondsInvestor, valuationDate);
                
                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }

            return fondSumup;
        }
    }
}
