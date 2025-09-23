using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class RealEstateService : IRealStateService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public RealEstateService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public decimal RealStateEngine(string ownerId, DateTime valuationDate)
        {

            var realStateSumup = _portfolioRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType == "RealEstate")
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            //var realStateSumup = investments
            //    .Where(investment => investment.InvestmentType == "RealEstate")
            //    .SelectMany(investment => investment.Transactions)
            //    .Sum(transaction => transaction.Value);

            return realStateSumup;
        }
    }
}
