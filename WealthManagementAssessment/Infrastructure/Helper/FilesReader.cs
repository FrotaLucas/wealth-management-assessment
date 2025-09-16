using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Infrastructure.Helper
{
    public class FilesReader : IFilesReader
    {

        private readonly AppConfig _appConfig;

        public FilesReader(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        public Dictionary<string, List<Investment>> ReadInvestments()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Investments))
            using (var csv = new CsvReader(reader, config))
            {
                var investments = csv.GetRecords<Investment>()
                     .GroupBy(investment => investment.InvestorId)
                     .ToDictionary(group => group.Key, group => group.ToList());

                return investments;
            }

        }

        public Dictionary<string, List<Transaction>> ReadTransactions()
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
                    .GroupBy(transacion => transacion.InvestmentId)
                    .ToDictionary(group => group.Key, group => group.ToList());

                return allTransactions;

            }

        }

        public Dictionary<string, List<Quote>> ReadQuotes()
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Quotes))

            using (var csv = new CsvReader(reader, config))
            {
                var allqQuotes = csv.GetRecords<Quote>()
                    .GroupBy(quote => quote.ISIN)
                    .ToDictionary(group => group.Key, group => group.OrderByDescending(quote => quote.Date).ToList());


                return allqQuotes;

            }

        }

    }
}