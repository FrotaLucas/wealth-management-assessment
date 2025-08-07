using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WealthManagementAssessment.Domain;

namespace WealthManagementAssessment.WealthManagementService
{
    public class AssetValuation
    {

        static string baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
        string fileInvestments = Path.Combine(baseDirectory, "Csv\\InvestmentsT.csv");
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\Transactions.csv");

        public string OwnerId { get; set; }

        //EndDate or ReferenceDate?
        public DateTime ValuationDate { get; set; }

        //try to use one single Investor and save all investments
        public Investor Investor { get; set; }


        public AssetValuation(string ownerId, DateTime valuationDate)
        {
            OwnerId = ownerId;
            ValuationDate = valuationDate;
        }
        public void FileReader()
        {
            Investor.Investments = File.ReadLines(fileInvestments)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(parts => parts[0] == OwnerId)
                .Select(parst => new Investment
                {
                    InvestmentId = parst[1],
                    InvestorId = parst[0],
                    InvestmentType = parst[2],
                    Isin = parst[3],
                    City = parst[4],
                    FondsInvestor = parst[5]
                }).ToList();

            Console.WriteLine($"total investor90: {Investor.Investments.Count}");

        }

    }
}
