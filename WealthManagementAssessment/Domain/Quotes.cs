using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Domain
{
    public class Quotes
    {
        public string Isin { get; set; }

        public DateTime Date { get; set; }

        public float PricePerShare { get; set; }
    }
}
