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



        public void ReadTransactions(List<Investment> investments)
        {
            foreach (var investment in investments)
            {
                investment.Transactions = File.ReadLines(fileTransactions)
                    .Skip(1)
                    .Select(line => line.Split(";"))
                    .Where(parts => parts[0] == investment.InvestmentId && DateTime.Parse(parts[2]) < ValuationDate) //cut out future transactions
                    .Select(parts => new Transaction
                    {
                        InvestmentId = parts[0],
                        Type = parts[1],
                        Date = DateTime.Parse(parts[2]),
                        Value = double.Parse(parts[3])

                    }).ToList();
            }
        }


        public List<Quote> ReadQuotes(List<Investment> investments)
        {
            var quotes = new List<Quote>();

            //Storing Quotes
            foreach (var investment in investments)
            {
                var invQuotes = File.ReadLines(fileQuotes)
                    .Skip(1)
                    .Select(parts => parts.Split(";"))
                    .Where(parts => parts[0] == investment.Isin && DateTime.Parse(parts[1]) < ValuationDate) //cut out unused quote range
                    .OrderByDescending(parts => parts[1])
                    .Select(parts => new Quote
                    {
                        Isin = parts[0],
                        Date = DateTime.Parse(parts[1]),
                        PricePerShare = float.Parse(parts[2])

                    }).ToList();

                quotes.AddRange(invQuotes);
            }

            Console.WriteLine($"Total Quotes: {quotes.Count}");


            int count = investments.Sum(i => i.Transactions.Count);
            Console.WriteLine($"Finish totaltransactions: {count}");

            return quotes;
        }
    }
}