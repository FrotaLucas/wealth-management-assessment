using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IPortfolioService
    {

        List<Investment> GetAllInvestmentsByInvestor(string investmentId, DateTime valuationDate);
    }
}
