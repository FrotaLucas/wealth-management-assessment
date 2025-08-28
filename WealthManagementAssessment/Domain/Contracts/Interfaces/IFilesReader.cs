using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IFilesReader
    {

        List<Investment> ReadInvestments(string ownerId);

        void ReadTransactions(List<Investment> investments, DateTime valuationDate);

        List<Quote> ReadQuotes(List<Investment> investments, DateTime valuationsDate); 
    }
}
