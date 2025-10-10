namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public CsvPathConfig CsvPath { get; set; } = new CsvPathConfig();
       
        public RiskProfileLimit RiskProfile {  get; set; } =  new RiskProfileLimit();

        public sealed class CsvPathConfig
        {
            public string Investments { get; set; } = default!;

            public string Transactions { get; set; } = default!;    

            public string Quotes { get; set; } = default!;

        }

        public sealed class RiskProfileLimit()
        {
            public decimal ConservativeUpperLimit { get; set; }

            public decimal ModerateUpperLimit { get; set;  }
        }

    }
}
