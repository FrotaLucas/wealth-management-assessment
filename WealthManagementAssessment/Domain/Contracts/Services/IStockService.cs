using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IStockService
    {
        //usar DECIMAL em tudo!!
        decimal StockEngine(List<Investment> investments, DateTime valuationDate);
    }
}
