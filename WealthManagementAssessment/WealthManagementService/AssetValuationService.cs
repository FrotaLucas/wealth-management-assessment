using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthManagementAssessment.WealthManagementService
{
    public class AssetValuationService
    {

        public double RealState { get; set; }

        public double Stocks { get; set; }

        public double Fonds { get; set; }

      
        public AssetValuationService(string ownerId)
        {
           
        }

    }
}
