namespace WealthManagementAssessment.Domain
{
    public class Investor
    {
        public string InvestorId { get; set; }

        public string InvestmentType { get; set; }

        public string Isin { get; set; }

        public string City { get; set; }

        public string FondsInvestor { get; set; }

        public List<Investment> Investments { get; set; }
    }
}
