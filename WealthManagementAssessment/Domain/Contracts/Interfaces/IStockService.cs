using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IStockService
    {
        //usar DECIMAL em tudo!!
        double StockEngine(List<Investment> investments, DateTime valuationDate);
    }
}
