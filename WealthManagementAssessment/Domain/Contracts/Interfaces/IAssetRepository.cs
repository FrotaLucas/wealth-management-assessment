using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetRepository
    {

        List<Investment> ReadInvestments(string ownerId);

        void ReadTransactions(List<Investment> investments);

        List<Quote> ReadQuotes(List<Investment> investments);

        void FilesReader();


        double RealStateEngine(List<Investment> investments);

        double StockEngine(List<Investment> investments);

        double FondEngine();

        void AssetEngine();
    }
}
