namespace WealthManagementAssessment.Application.Models
{
    public class InvestorBalanceResult
    {
        public decimal RealStateBalance { get; set; }

        public decimal StockBalance { get; set; }

        public decimal FondBalance { get; set; }

        public decimal TotalBalance => RealStateBalance + StockBalance + FondBalance;
    }
}
