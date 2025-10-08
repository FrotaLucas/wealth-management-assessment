using WealthManagementAssessment.Domain.Enums;

namespace WealthManagementAssessment.Infrastructure.DataProvider
{
    public class InvestmentData
    {
        public string InvestorId { get; set; } = default!;

        public string InvestmentId { get; set; } = default!;

        public InvestmentTypeEnum InvestmentType { get; set; }

        public string ISIN { get; set; } = default!;

        public string City { get; set; } = default!;

        public string FondsInvestor { get; set; } = default!;
    }
}