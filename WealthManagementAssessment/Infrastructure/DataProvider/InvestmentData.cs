using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Infrastructure.DataProvider
{
    public class InvestmentData
    {
        public string InvestorId { get; set; }

        public string InvestmentId { get; set; }

        public InvestmentTypeEnum InvestmentType { get; set; }

        public string ISIN { get; set; }

        public string City { get; set; }

        public string FondsInvestor { get; set; }

    }
}
