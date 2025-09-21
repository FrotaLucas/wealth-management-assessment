using System.Runtime.CompilerServices;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class ProfileService
    {

        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IFondService _fondService;

        private readonly IRealStateService _realStateService;

        private readonly IStockService _stockService;

        public ProfileService(IPortfolioRepository portfolioRepository, IRealStateService realStateService, IStockService stockService)
        {
            _portfolioRepository = portfolioRepository;
            _realStateService = realStateService;
            _stockService = stockService;
        }

        public void ProfileEngine(string ownerId)
        {
            //talvel possa deletar de portfolio
            //var allInVestments = _portfolioRepository.GetAllInvestments();

            //pegar data de hoje
            DateTime date = DateTime.Parse("2025-09-21");

            decimal investmentFond = _fondService.FondEngine(ownerId, date);
            decimal investmentStock = _stockService.StockEngine(ownerId, date);
            decimal investmentRealState = _realStateService.RealStateEngine(ownerId, date);

            decimal totalWallet = investmentFond + investmentStock + investmentRealState;

            //peso fond
            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = _portfolioRepository.GetAllInvestmentsByInvestor(ownerId, date)
                .Where(investment => investment.InvestmentType == "Fonds")
                .ToList();


            decimal riskkSum = 0;
            foreach( var fond in fonds)
            {

                decimal investRealState = _realStateService.RealStateEngine(fond.FondsInvestor, date);
                decimal investStock = _stockService.StockEngine(fond.FondsInvestor, date);
                decimal totInvest = investStock + investRealState;

                riskkSum = riskkSum + (investRealState * 1 + investStock * 2) / totInvest;

            }

            decimal riskBalance = riskkSum / fonds.Count; 

            //lista 
            //fond0
            //

            decimal profile = (investmentRealState * 1  + investmentStock * 2   )/ totalWallet; 

        }
        
    }
}
