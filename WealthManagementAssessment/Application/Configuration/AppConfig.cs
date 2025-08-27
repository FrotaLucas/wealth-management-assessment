using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public CsvPaths CsvsPaths { get; set; } = new CsvPaths();

        public sealed class CsvPaths
        {
            public string InvestmentsPath { get; set; }

            public string TransactionsPath { get; set; }

            public string QuotesPath { get; set; }

        }
    }
}
