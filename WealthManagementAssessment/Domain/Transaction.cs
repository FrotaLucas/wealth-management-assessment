using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Domain
{
    public class Transaction
    {
        public int InvestmentId { get; set; }

        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public double Value { get; set; }   


    }

}
