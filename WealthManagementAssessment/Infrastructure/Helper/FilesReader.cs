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


        //create better method name
        public List<Investment> ReadAllFondsByInvestor(string ownerId)
        {
            List<Investment> listOfFonds = new List<Investment>();
            
            List<string> fonds = ReadInvestmentByInvestor(ownerId)
                .Where(investment => investment.InvestmentType.Equals("Fonds"))
                .Select(investment => investment.FondsInvestor)
                .Distinct()
                .ToList();
           

            List<Investment> investments = ReadInvestments();



            foreach(var investment in investments)
            {
                foreach (string fond in fonds)
                {
                    if(investment.InvestorId == fond)
                      listOfFonds.Add(investment);
                }
            }



            return listOfFonds;
        }

        public Dictionary<string, List<Investment>> GetDictionary(string ownerId, DateTime valuationDate) 
        {
            //List<Investment> allInvestments = ReadInvestments();


            //pegar lista de investimentos do fundo
            List<Investment> allInvestments = ReadAllFondsByInvestor(ownerId); 

            var dictionary = allInvestments
                .GroupBy(investment => investment.InvestorId)
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach( var kvp in dictionary) 
            {
                var investments = kvp.Value;

                ReadTransactions(investments, valuationDate);
            }
                 
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