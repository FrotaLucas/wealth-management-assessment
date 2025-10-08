using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Entities;
using WealthManagementAssessment.Infrastructure.DataProvider;

namespace WealthManagementAssessment.Infrastructure.Helper
{
    public class DataSource : IDataSource
    {
        private readonly AppConfig _appConfig;

        public DataSource(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        public Dictionary<string, List<InvestmentData>> ReadInvestments()
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
                var investments = csv.GetRecords<InvestmentData>()
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