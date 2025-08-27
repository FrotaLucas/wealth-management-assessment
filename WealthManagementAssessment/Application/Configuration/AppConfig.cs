using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public DataFileCsv DataFilePath { get; set; } = new DataFileCsv();

        public sealed class DataFileCsv
        {
            public string InvestmentsPath { get; set; }

            public string TransactionsPath { get; set; }

            public string QuotesPath { get; set; }

        }
    }
}
