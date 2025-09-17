using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IAssetRepository
    {
        //double RealStateEngine(List<Investment> investments);

        //double StockEngine(List<Investment> investments, DateTime valuationDate);

        //double FondEngine(string ownerId ,DateTime valuationDate);

        List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate);

        Dictionary<string, List<Investment>> GetAllInvestmentsByFonds(string ownerId, DateTime valuationDate);


        //NAO SERIA MELHOR DEFINIR COMO METODO AO INVES DE PROPRIEDADE ?????
        IReadOnlyDictionary<string, List<Investment>> InvestmentsByOwnerId { get; }

        IReadOnlyDictionary<string, List<Quote>> QuotesByIsin { get; }

        IReadOnlyDictionary<string, List<Transaction>> TransactionsByInvestmentId { get; }

    }
}
