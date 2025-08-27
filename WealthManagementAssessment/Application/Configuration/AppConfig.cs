using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Application.Configuration
{
    public class AppConfig
    {

        public DataFileCsv DataFile { get; set; }

        public sealed class DataFileCsv
        {
            public string InvestmentsFile { get; set; }

            public string TransactionsFile { get; set; }

            public string QuotesFile { get; set; }

        }
    }
}
