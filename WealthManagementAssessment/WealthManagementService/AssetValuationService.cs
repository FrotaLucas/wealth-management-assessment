using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WealthManagementAssessment.Domain;

namespace WealthManagementAssessment.WealthManagementService
{
    public class AssetValuationService
    {


        static string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\Investments.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\Transactions.csv");

        List<Investment> selectedInvestments = new List<Investment>();
        List<Transaction> selectedEstate = new List<Transaction>();
        List<Transaction> selectedBuilding = new List<Transaction>();

        public string OwnerId { get; set; }

        public AssetValuationService(string ownerId)
        {
            OwnerId = ownerId;
        }







        public void RealStateEngine()
        {
            Console.WriteLine(fileInvestments);
            Console.WriteLine(fileTransactions);

        }
    }
}
