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

        public Dictionary<string, List<Investment>>  ReadInvestmentsv2()
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

            return investments.GroupBy(investment => investment.InvestorId)
                .ToDictionary(group => group.Key, group => group.ToList());


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


        public Dictionary<string, List<Investment>> GetDictionary(string ownerId, DateTime valuationDate) 
        {
            //unique id


            DateTime t1 = DateTime.Now;
            var fondsId = new HashSet<string>(ReadInvestmentByInvestor(ownerId)
                .Where(investment => investment.InvestmentType.Equals("Fonds"))
                .Select(investment => investment.FondsInvestor));
            DateTime t2 = DateTime.Now;
            Console.WriteLine($" 1try: {t2 - t1}");

            //200 fundos 


            DateTime t3 = DateTime.Now;
            List<Investment> allFondsOfInvestor = ReadInvestments()
                .Where(investment => fondsId.Contains(investment.InvestorId))
                .ToList();
            DateTime t4 = DateTime.Now;
            Console.WriteLine($"2 try: {t4 - t3}");

            DateTime t5 = DateTime.Now;
            var dictionary = allFondsOfInvestor
                .GroupBy(investment => investment.InvestorId)
                .ToDictionary(group => group.Key, group => group.ToList());
            DateTime t6= DateTime.Now;
            Console.WriteLine($" 3 tryt: {t6 - t5}");



            DateTime t7 = DateTime.Now;
            var allTransations = ReadTransactionsV2( );
            DateTime t8 = DateTime.Now;
            Console.WriteLine($" 4 tryt: {t8 - t7}");

            DateTime t9 = DateTime.Now;
            foreach ( var kvp in dictionary) 
            {
                var investments = kvp.Value;


                foreach (var investment in investments)
                {
                    if (allTransations.TryGetValue(investment.InvestmentId, out var transactions))
                        investment.Transactions = transactions.Where(transaction => transaction.Date < valuationDate).ToList();
                    else
                        investment.Transactions = new List<Transaction>();
                }
            }

            DateTime t10 = DateTime.Now;
            Console.WriteLine($" 5 try: {t10 - t9}");


          


            return dictionary;    
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
                //N --> N +1 
                // id inves --> list de transacao 

                var allTransactions = csv.GetRecords<Transaction>()
                 .Where(t => t.Date <= valuationDate)
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

        public Dictionary<string, List<Transaction>> ReadTransactionsV2()
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
                //N --> N +1 
                // id inves --> list de transacao 

                Dictionary<string, List<Transaction>> allTransactions = csv.GetRecords<Transaction>()
                 //.Where(t => t.Date <= valuationDate)
                 .GroupBy(transaction => transaction.InvestmentId)
                 .ToDictionary(group => group.Key, group => group.ToList());

                return allTransactions;
                    
            }

          
           

             
        }




        public  Dictionary< string, List<Quote>> ReadQuotesV2()
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
                    .GroupBy(quote => quote.ISIN)
                    .ToDictionary(quote => quote.Key, quote => quote.ToList());

                return allQuotes;

            }

            //Console.WriteLine($"Total Quotes: {quotes.Count}");
            //int count = investments.Sum(i => i.Transactions.Count);
            //Console.WriteLine($"Finish totaltransactions: {count}");

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
                    .Where(quote => quote.Date <= valuationDate)
                    .GroupBy(quote => quote.ISIN)
                    .ToDictionary(quote => quote.Key, quote => quote.ToList());

                foreach (var investment in investments)
                {
                    if (allQuotes.TryGetValue(investment.ISIN, out var listOfQuotes))
                    {
                        var invQuotes = listOfQuotes
                            .OrderByDescending(quote => quote.Date)
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