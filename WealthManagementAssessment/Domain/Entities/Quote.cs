namespace WealthManagementAssessment.Domain.Entities
{
    public class Quote
    {
        public string ISIN { get; set; } = default!;    

        public DateTime Date { get; set; }

        public decimal PricePerShare { get; set; } = default!;
    }
}
