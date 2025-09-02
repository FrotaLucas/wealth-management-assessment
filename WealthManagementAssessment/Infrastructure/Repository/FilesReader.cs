using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Repository
{
    public class FilesReader : IFilesReader
    {

        private readonly AppConfig _appConfig;

        public FilesReader(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        public List<Investment> ReadInvestments(string ownerId)
        {
            var investments = File.ReadLines(_appConfig.CsvPath.Investments)
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

            //Console.WriteLine($"total investment of investor90: {investments.Count}");

            return investments;
        }



        public void ReadTransactions(List<Investment> investments, DateTime valuationDate)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim, // remove espaços extras nos cabeçalhos e valores
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Transactions))

            using (var csv = new CsvReader(reader, config))
            {
                //LER TODO ARQUIVO transactions de 700K linhas
                var allTransactions = csv.GetRecords<Transaction>()
                    .Where(t => t.Date < valuationDate) 
                    .ToList();

                foreach (var investment in investments)
                {
                    investment.Transactions = allTransactions
                        .Where(t => t.InvestmentId == investment.InvestmentId)
                        .ToList();
                }
            }

        }


        //read quotes of Investor
        public List<Quote> ReadQuotes(List<Investment> investments, DateTime valuationDate)
        {
            var quotes = new List<Quote>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim, // remove espaços extras nos cabeçalhos e valores
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Quotes))

            using (var csv = new CsvReader(reader, config))
            {
                List<Quote> allQuotes = csv.GetRecords<Quote>().ToList();

                foreach (var investment in investments)
                {
                    var invQuotes = allQuotes
                        .Where(parts => parts.ISIN == investment.Isin && parts.Date < valuationDate) //cut out unused quote range
                        .OrderBy(quote => quote.ISIN)
                        .ThenByDescending(quote => quote.Date)
                        .ToList();
                    quotes.AddRange(invQuotes);
                }
            }


            //Console.WriteLine($"Total Quotes: {quotes.Count}");
            //int count = investments.Sum(i => i.Transactions.Count);
            //Console.WriteLine($"Finish totaltransactions: {count}");

            return quotes;
        }
    }
}