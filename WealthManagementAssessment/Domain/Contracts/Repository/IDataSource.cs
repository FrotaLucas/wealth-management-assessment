using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Repository
{
    public interface IDataSource
    {
        Dictionary<string, List<Investment>> ReadInvestments();

        Dictionary<string, List<Transaction>> ReadTransactions();

        Dictionary<string, List<Quote>> ReadQuotes();
    }
}
