using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFilesReader
    {

        List<Investment> ReadInvestmentByInvestor(string ownerId);

        List<Investment> ReadInvestments();

        Dictionary<string, List<Transaction>> ReadTransactions();

        Dictionary<string, List<Quote>> ReadQuotes();

        Dictionary<string, List<Investment>> GetDictionary(string ownerId, DateTime valuationTime);

    }
}
