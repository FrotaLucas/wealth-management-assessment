using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFilesReader
    {
        Dictionary<string, List<Investment>> ReadInvestments();

        Dictionary<string, List<Transaction>> ReadTransactions();

        Dictionary<string, List<Quote>> ReadQuotes();
    }
}
