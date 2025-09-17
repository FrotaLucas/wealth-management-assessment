using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IPortifolioService
    {

        List<Investment> GetAllInvestmentsByInvestor(string investmentId, DateTime valuationDate);
    }
}
