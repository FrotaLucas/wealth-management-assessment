using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Infrastructure.DataProvider;

namespace WealthManagementAssessment.Domain.Contracts.Repository
{
    public interface IDataSource
    {
        Dictionary<string, List<InvestmentData>> ReadInvestments();

        Dictionary<string, List<Transaction>> ReadTransactions();

        Dictionary<string, List<Quote>> ReadQuotes();
    }
}
