using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Domain
{
    public class Investor
    {
        public string InvestorId { get; set; }

        public List<Investment> Investments { get; set; }
    }
}
