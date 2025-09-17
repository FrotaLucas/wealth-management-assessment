using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class RealStateService : IRealStateService
    {
        public double RealStateEngine(List<Investment> investments)
        {

            var realStateSumup = investments
                .Where(investment => investment.InvestmentType == "RealEstate")
                .SelectMany(investment => investment.Transactions)
                .Sum(transaction => transaction.Value);

            return realStateSumup;
        }
    }
}
