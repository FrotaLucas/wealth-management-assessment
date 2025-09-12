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

        Dictionary<string, List<Quote>> ReadQuotesV2();

        Dictionary<string, List<Investment>> ReadInvestmentsv2();

        Dictionary<string, List<Transaction>> ReadTransactionsV2();

    }
}
