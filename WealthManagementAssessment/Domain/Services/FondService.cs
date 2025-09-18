using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class FondService : IFondService
    {
        private readonly IPortfolioRepository _assetRepository;

        private readonly IRealStateService _realStateService;

        private readonly IStockService _stockService;
        
        public FondService(IPortfolioRepository assetRepository, IRealStateService realStateService, IStockService stockService)
        {
            _assetRepository = assetRepository;
            _realStateService = realStateService;
            _stockService = stockService;
        }

        public decimal FondEngine(string ownerId, DateTime valuationDate)
        {
            decimal fondSumup = 0;

            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType == "Fonds")
                .ToList();

            Dictionary<string, List<Investment>> dictionary = _assetRepository.GetAllFondsByInvestor(ownerId, valuationDate);

            foreach (var fond in fonds)
            {
                decimal totalPercentage = fond.Transactions.Sum(t => t.Value);

                dictionary.TryGetValue(fond.FondsInvestor, out var investmentsOfFound);
                decimal realStateSumup = _realStateService.RealStateEngine(fond.FondsInvestor, valuationDate);
                decimal stockSumup = _stockService.StockEngine(investmentsOfFound, valuationDate);
                
                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }

            return fondSumup;
        }
    }
}
