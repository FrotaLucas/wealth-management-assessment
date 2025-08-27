using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.Domain.Entities
{
    public class Quote
    {
        public string Isin { get; set; }

        public DateTime Date { get; set; }

        public float PricePerShare { get; set; }
    }
}
