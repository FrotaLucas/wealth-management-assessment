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
        string fileTransactions = Path.Combine(baseDirectory, "Csv\\TransactionsT.csv");

        public string OwnerId { get; set; }

        //EndDate or ReferenceDate?
        public DateTime ValuationDate { get; set; }

        public Investor Investor { get; set; } = new Investor();

        public AssetValuation(string ownerId, DateTime valuationDate)
        {
            OwnerId = ownerId;
            ValuationDate = valuationDate;
        }
        public void FilesReader()
        {

            Investor.Investments = File.ReadLines(fileInvestments)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(parts => parts[0] == OwnerId)
                .OrderByDescending(parts => parts[1]) //92.. 82.. 81.. 
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

            var trans = new Transaction();

            foreach (var investment in Investor.Investments)
            {
                investment.Transactions = File.ReadLines(fileTransactions)
                    .Skip(1)
                    .Select(line => line.Split(";"))
                    .Where(parts => parts[0] == investment.InvestmentId)
                    .Select(parts => new Transaction
                    {
                        InvestmentId = parts[0],
                        Type = parts[1],
                        //DateTime = DateTime.Parse(parts[2]),
                        Value = Double.Parse(parts[3])

                    }).ToList();
            }


            //foreach (var investment in Investor.Investments)
            //{
            //    Console.WriteLine($"investmentId: {investment.InvestmentId}\n");
            //    Console.WriteLine($"investorId: {investment.InvestorId}\n");
            //    Console.WriteLine($"invesment Type: {investment.InvestmentType}\n");
            //    Console.WriteLine($"City: {investment.City} \n");
            //}

            foreach (var investment in Investor.Investments)
            {
                foreach (var transaction in investment.Transactions)
                {
                    Console.WriteLine($"transactionsId: {transaction.InvestmentId}\n");
                    Console.WriteLine($"type: {transaction.Type}\n");
                    Console.WriteLine($"Value: {transaction.Value}\n");
                    //Console.WriteLine($"City: {transaction.DateTime} \n");
                }
            }


        }

    }
}
