namespace WealthManagementAssessment.Domain.Entities
{
    public class Stock : Investment
    {
        public string ISIN { get; set; } = default;
    }
}
