using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFilesReader
    {

        List<Investment> ReadInvestmentByInvestor(string ownerId);

        List<Investment> ReadInvestments();

        void ReadTransactions(List<Investment> investments, DateTime valuationDate);

        List<Quote> ReadQuotes(List<Investment> investments, DateTime valuationsDate); 

        Dictionary<string, List<Investment>> GetDictionary(string ownerId, DateTime valuationTime);

        List<Investment> ReadAllFondsByInvestor(string ownerId);
    }
}
