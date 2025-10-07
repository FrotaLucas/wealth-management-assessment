namespace WealthManagementAssessment.Domain.Entities
{
    public class Investor
    {
        public string InvestorId { get; set; } = default!;

        public List<Investment> Investments { get; set; } = new List<Investment>();
    }
}
