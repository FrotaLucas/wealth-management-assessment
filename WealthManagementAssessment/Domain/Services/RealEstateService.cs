using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;

namespace WealthManagementAssessment.Domain.Services
{
    public class RealEstateService : IRealEstateService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public RealEstateService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public decimal RealStateEngine(string ownerId, DateTime valuationDate)
        {

            var realStateSumup = _portfolioRepository.GetRealEstatesByInvestor(ownerId, valuationDate)
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            return realStateSumup;
        }
    }
}