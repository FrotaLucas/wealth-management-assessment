namespace WealthManagementAssessment.Domain.Entities
{
    public class Transaction
    {
        public string InvestmentId { get; set; } = default!;

        public string Type { get; set; } = default!;

        public DateTime Date { get; set; }

        public decimal Value { get; set; } = default!;


    }

}
