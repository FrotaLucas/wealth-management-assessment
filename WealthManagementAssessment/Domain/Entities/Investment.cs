using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Domain.Entities
{
    
    public abstract class Investment
    {
        public string InvestorId { get; set; } = default!;

        public string InvestmentId { get; set; } = default!;

        //eliminar depois esse campo!!!
        public InvestmentTypeEnum InvestmentType { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();  


    }
}
