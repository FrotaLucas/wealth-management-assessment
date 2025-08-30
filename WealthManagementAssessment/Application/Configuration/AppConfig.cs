namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public CsvPaths CsvsPaths { get; set; } = new CsvPaths();

        public sealed class CsvPaths
        {
            public string Investments { get; set; }

            public string Transactions { get; set; }

            public string Quotes { get; set; }

        }
    }
}
