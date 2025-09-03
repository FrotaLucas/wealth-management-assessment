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


        //carregar todos os dados sem discriminar o INvestorId
        public List<Investment> ReadInvestmentByInvestor(string ownerId)
        {

            List<Investment> investments = new List<Investment>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim, // remove espaços extras nos cabeçalhos e valores
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
                TrimOptions = TrimOptions.Trim, // remove espaços extras nos cabeçalhos e valores
            };

            using (var reader = new StreamReader(_appConfig.CsvPath.Transactions))

            using (var csv = new CsvReader(reader, config))
            {
                //LER TODO ARQUIVO transactions de 700K linhas
                //var allTransactions = csv.GetRecords<Transaction>()
                //    .Where(t => t.Date < valuationDate)
                //    .ToList();


                ////id investmentId e value eh uma lista de transactions

                //// N N +1 
                //foreach (var investment in investments)
                //{
                //    investment.Transactions = allTransactions
                //        .Where(t => t.InvestmentId == investment.InvestmentId)
                //        .ToList();
                //}


                //new code 
                var allTransactions = csv.GetRecords<Transaction>()
                 .Where(t => t.Date < valuationDate)
                 .GroupBy(transaction => transaction.InvestmentId)
                 .ToDictionary(group => group.Key, group => group.ToList());

                foreach (var investment in investments)
                {
                    if (allTransactions.TryGetValue(investment.InvestmentId, out var transactions))
                    {
                        investment.Transactions = transactions;
                    }
                    else
                    {
                        investment.Transactions = new List<Transaction>();
                    }
                }

            }

        }


        //read quotes of Investor

        public List<Investment> ReadInvestments()
        {

            List<Investment> investments = new List<Investment>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim, // remove espaços extras nos cabeçalhos e valores
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
                TrimOptions = TrimOptions.Trim, // remove espaços extras nos cabeçalhos e valores
            };


            //new code 
            using (var reader = new StreamReader(_appConfig.CsvPath.Quotes))

            using (var csv = new CsvReader(reader, config))
            {
                var allQuotes = csv.GetRecords<Quote>()
                    .GroupBy(t => t.ISIN)
                    .ToDictionary(line => line.Key, line => line.ToList());

                foreach (var investment in investments)
                {

                    if (allQuotes.TryGetValue(investment.ISIN, out var listOfQuotes))
                    {

                        var invQuotes = listOfQuotes
                            .Where(parts => parts.Date < valuationDate) //cut out unused quote range
                            .OrderBy(quote => quote.Date)
                            .ToList();
                        quotes.AddRange(invQuotes);
                    }

                    else
                    {
                        quotes.AddRange(new List<Quote>());
                    }

                }


        }



            //old code
            //using (var reader = new StreamReader(_appConfig.CsvPath.Quotes))

            //using (var csv = new CsvReader(reader, config))
            //{
            //    List<Quote> allQuotes = csv.GetRecords<Quote>().ToList();

            //    foreach (var investment in investments)
            //    {
            //        var invQuotes = allQuotes
            //            .Where(parts => parts.ISIN == investment.ISIN && parts.Date < valuationDate) //cut out unused quote range
            //            .OrderBy(quote => quote.ISIN)
            //            .ThenByDescending(quote => quote.Date)
            //            .ToList();

            //        quotes.AddRange(invQuotes);
            //    }
            //}


            //Console.WriteLine($"Total Quotes: {quotes.Count}");
            //int count = investments.Sum(i => i.Transactions.Count);
            //Console.WriteLine($"Finish totaltransactions: {count}");

            return quotes;
        }

}
}