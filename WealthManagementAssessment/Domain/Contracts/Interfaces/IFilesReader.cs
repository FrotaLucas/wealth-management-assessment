using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFilesReader
    {

        List<Investment> ReadInvestmentByInvestor(string ownerId);

        List<Investment> ReadInvestments();

        Dictionary<string, List<Investment>> GetDictionary(DateTime valuationTime);

        void ReadTransactions(List<Investment> investments, DateTime valuationDate);

        List<Quote> ReadQuotes(List<Investment> investments, DateTime valuationsDate); 
    }
}
