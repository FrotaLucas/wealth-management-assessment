namespace WealthManagementAssessment.Domain.Entities
{
    public class Quote
    {
        public string Isin { get; set; }

        public DateTime Date { get; set; }

        public float PricePerShare { get; set; }
    }
}
