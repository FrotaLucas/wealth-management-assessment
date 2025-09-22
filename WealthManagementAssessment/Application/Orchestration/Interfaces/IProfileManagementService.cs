using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Application.Orchestration.Interfaces
{
    public interface IProfileManagementService
    {

        List<Investment> GetAllInvestmentsByInvestor(string investmentId, DateTime valuationDate);
    }
}
