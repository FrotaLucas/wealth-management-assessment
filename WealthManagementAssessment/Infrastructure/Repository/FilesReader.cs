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


        //NAO DEVERIA USAR MAIS ESSE METODO
        public List<Investment> ReadInvestmentByInvestor(string ownerId)
        {

            List<Investment> investments = new List<Investment>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Investments))

            using (var csv = new CsvReader(reader, config))
            {
                investments = csv.GetRecords<Investment>()
                     .Where(inv => inv.InvestorId == ownerId)
                     .ToList();

            }

            return investments;

            //Console.WriteLine($"total investment of investor90: {investments.Count}");

            //return investmentss;
        }


        public void ReadTransactions(List<Investment> investments, DateTime valuationDate)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim, 
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Transactions))

            using (var csv = new CsvReader(reader, config))
            {

                var allTransactions = csv.GetRecords<Transaction>()
                 .Where(t => t.Date < valuationDate)
                 .GroupBy(transaction => transaction.InvestmentId)
                 .ToDictionary(group => group.Key, group => group.ToList());

                foreach (var investment in investments)
                {
                    if (allTransactions.TryGetValue(investment.InvestmentId, out var transactions))
                        investment.Transactions = transactions;
                    else
                        investment.Transactions = new List<Transaction>();
                }
            }

        }

        public List<Investment> ReadInvestments()
        {
            List<Investment> investments = new List<Investment>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim, 
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Investments))
            using (var csv = new CsvReader(reader, config))
            {
                investments = csv.GetRecords<Investment>()
                     .ToList();
            }

            return investments;
        }

        public List<Quote> ReadQuotes(List<Investment> investments, DateTime valuationDate)
        {
            var quotes = new List<Quote>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Quotes))
            using (var csv = new CsvReader(reader, config))
            {
                var allQuotes = csv.GetRecords<Quote>()
                    .Where(quote => quote.Date < valuationDate)
                    .GroupBy(quote => quote.ISIN)
                    .ToDictionary(quote => quote.Key, quote => quote.ToList());

                foreach (var investment in investments)
                {
                    if (allQuotes.TryGetValue(investment.ISIN, out var listOfQuotes))
                    {
                        var invQuotes = listOfQuotes
                            .OrderBy(quote => quote.Date)
                            .ToList();

                        quotes.AddRange(invQuotes);
                    }

                    else
                        quotes.AddRange(new List<Quote>());
                }
            }

            //Console.WriteLine($"Total Quotes: {quotes.Count}");
            //int count = investments.Sum(i => i.Transactions.Count);
            //Console.WriteLine($"Finish totaltransactions: {count}");

            return quotes;
        }

    }
}