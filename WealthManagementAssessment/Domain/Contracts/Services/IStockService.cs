namespace WealthManagementAssessment.Domain.Contracts.Interfaces
{
    public interface IStockService
    {
        decimal StockEngine(string ownerid, DateTime valuationDate);
    }
}
