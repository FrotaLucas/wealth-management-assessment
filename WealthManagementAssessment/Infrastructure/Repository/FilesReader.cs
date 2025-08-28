using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class FilesReader
    {

        public readonly string fileInvestments;
        public readonly string fileTransactions;
        public readonly string fileQuotes;

        public FilesReader(string baseDirectory)
        {
            fileInvestments = Path.Combine(baseDirectory, "Infrastructure\\InvestmentsT.csv");

            fileTransactions = Path.Combine(baseDirectory, "Infrastructure\\TransactionsT.csv");

            fileQuotes = Path.Combine(baseDirectory, "Infrastructure\\Quotes.csv");
        }



        public List<Investment> ReadInvestments(string ownerId)
        {
            var investments = File.ReadLines(fileInvestments)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(parts => parts[0] == ownerId)
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

            Console.WriteLine($"total investment of investor90: {investments.Count}");

            return investments;
        }
    }
}