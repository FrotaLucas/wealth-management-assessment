namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public CsvPathConfig CsvsPaths { get; set; } = new CsvPathConfig();

        public sealed class CsvPathConfig
        {
            public string Investments { get; set; }

            public string Transactions { get; set; }

            public string Quotes { get; set; }

        }
    }
}
