using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class RealStateService : IRealStateService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public RealStateService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public decimal RealStateEngine(List<Investment> investments)
        {

            

            var realStateSumup = investments
                .Where(investment => investment.InvestmentType == "RealEstate")
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            return realStateSumup;
        }
    }
}
