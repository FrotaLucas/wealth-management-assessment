using System.Globalization;
using System.Text.RegularExpressions;
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


        public Dictionary<string, List<Investment>> GetDictionary(string ownerId, DateTime valuationDate)
        {
            //unique id
            //var fondsId = new HashSet<string>(ReadInvestmentByInvestor(ownerId)
            //    .Where(investment => investment.InvestmentType.Equals("Fonds"))
            //    .Select(investment => investment.FondsInvestor));


            //List<Investment> allFondsOfInvestor = ReadInvestments()
            //    .Where(investment => fondsId.Contains(investment.InvestorId))
            //    .ToList();

            //var dictionary = allFondsOfInvestor
            //    .GroupBy(investment => investment.InvestorId)
            //    .ToDictionary(group => group.Key, group => group.ToList());

            //foreach( var kvp in dictionary) 
            //{
            //    var investments = kvp.Value;

            //    ReadTransactions(investments, valuationDate);
            //}

            //return dictionary;

            return new Dictionary<string, List<Investment>>();
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