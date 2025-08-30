namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public CsvPath CsvsPaths { get; set; } = new CsvPath();

        public sealed class CsvPath
        {
            public string Investments { get; set; }

            public string Transactions { get; set; }

            public string Quotes { get; set; }

        }
    }
}
