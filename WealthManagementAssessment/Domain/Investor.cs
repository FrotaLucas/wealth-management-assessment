namespace WealthManagementAssessment.Domain
{
    public class Investor
    {
        public string InvestorId { get; set; }

        public List<Investment> Investments { get; set; } = new List<Investment>();
    }
}
