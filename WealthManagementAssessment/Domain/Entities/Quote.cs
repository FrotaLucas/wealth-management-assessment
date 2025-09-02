namespace WealthManagementAssessment.Domain.Entities
{
    public class Quote
    {
        public string ISIN { get; set; }

        public DateTime Date { get; set; }

        public float PricePerShare { get; set; }
    }
}
