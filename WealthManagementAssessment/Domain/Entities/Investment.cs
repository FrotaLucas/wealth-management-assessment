using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Domain.Entities
{
    public class Investment
    {
        public string InvestorId { get; set; }

        public string InvestmentId { get; set; }

        public InvestmentTypeEnum InvestmentType { get; set; }

        public string ISIN { get; set; }

        public string City { get; set; }

        public string FondsInvestor { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();  


    }
}
