using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Repository
{
    public interface IPortfolioRepository
    {
        //decimal RealStateEngine(List<Investment> investments);

        //decimal StockEngine(List<Investment> investments, DateTime valuationDate);

        //decimal FondEngine(string ownerId ,DateTime valuationDate);

        List<Investment> GetAllInvestmentsByInvestor(string ownerId, DateTime valuationDate);

        Dictionary<string, List<Investment>> GetAllFondsByInvestor(string ownerId, DateTime valuationDate);


        //NAO SERIA MELHOR DEFINIR COMO METODO AO INVES DE PROPRIEDADE ?????
        IReadOnlyDictionary<string, List<Investment>> InvestmentsByOwnerId { get; }

        IReadOnlyDictionary<string, List<Quote>> QuotesByIsin { get; }

        IReadOnlyDictionary<string, List<Transaction>> TransactionsByInvestmentId { get; }

    }
}
