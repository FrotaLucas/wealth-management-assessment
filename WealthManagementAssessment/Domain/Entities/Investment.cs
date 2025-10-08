using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Domain.Entities
{
    public abstract class Investment
    {
        public string InvestorId { get; set; } = default!;

        public string InvestmentId { get; set; } = default!;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();  


    }
}
