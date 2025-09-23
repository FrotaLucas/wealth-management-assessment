using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Application.Orchestration;
using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;
using WealthManagementAssessment.Domain.Services;
using WealthManagementAssessment.Infrastructure.Helper;
using WealthManagementAssessment.Infrastructure.Repository;

class Program
{
    private static void Main(string[] args)
    {

        string baseDir = AppContext.BaseDirectory;
        string projectDirectory = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent.FullName;


        string investorId1 = "Investor90";
        string dateString = "2028-12-31";


        //var line = Console.ReadLine();
        //var steps = line.Split(";");
        DateTime date1 = DateTime.Parse(dateString);

        //Console.WriteLine("DateTime format: " + date1);
        //1/16/2016 12:00:00 AM

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration( (ctx, cfg) =>
            { 
                cfg.SetBasePath(AppContext.BaseDirectory);

                cfg.AddJsonFile(Path.Combine("Application","appsettings.json"), optional: true, reloadOnChange: true );
                cfg.AddJsonFile(Path.Combine("Application", $"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json"), optional: true, reloadOnChange: true);
                
                cfg.AddEnvironmentVariables();

            })
            .ConfigureServices( (ctx, services) =>
            {
                services.Configure<AppConfig>(options =>
                {
                    ctx.Configuration.Bind(options);

                    options.CsvPath.Investments = Path.Combine(projectDirectory, options.CsvPath.Investments);
                    options.CsvPath.Transactions = Path.Combine( projectDirectory, options.CsvPath.Transactions );
                    options.CsvPath.Quotes = Path.Combine(projectDirectory, options.CsvPath.Quotes);

                });

                services.AddSingleton<IFilesReader, FilesReader>();
                services.AddSingleton<IPortfolioRepository, PortfolioRepository>();
                services.AddSingleton<IAssetManagementService, AssetManagementService>();
                services.AddSingleton<IStockService, StockService>();
                services.AddSingleton<IRealStateService, RealEstateService>();
                services.AddSingleton<IProfileService, ProfileService>();
                services.AddSingleton<IFondService, FondService>(); 

            })
            .Build();


        var assetManagemetn = host.Services.GetRequiredService<IAssetManagementService>();

        string greeting = DateTime.Now.Hour < 12 ? "Good morning" : DateTime.Now.Hour < 18 ? "Good afternoon" : "Good evenning";

        Console.WriteLine($"\n                    === {greeting}, welcome to the Wealth Management Platform ===");
        Console.WriteLine($"\n=== Enter a valid date and InvestmentId to view your portfolio size. (ex. 2025-07-20;Investor90) === \n");

        var line = Console.ReadLine();

        while (!string.IsNullOrWhiteSpace(line))
        {
            var input = line.Split(";");

            DateTime date = DateTime.Parse(input[0]);
            string investorId = input[1];

            bool showMenu = true; // flag para sair do menu e voltar para novo ID/data
            while (showMenu)
            {
                Console.WriteLine("Choose your investment type: \n");
                Console.WriteLine(" 1 - RealState Asset");
                Console.WriteLine(" 2 - Stock Asset");
                Console.WriteLine(" 3 - Fund Asset");
                Console.WriteLine(" 4 - Total Asset");
                Console.WriteLine(" 5 - Check your risk profile");
                Console.WriteLine(" 0 - Enter new InvestmentId and Date\n"); // nova opção

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        decimal realEstate = assetManagemetn.GetRealEstateAsset(investorId, date);
                        Console.WriteLine($"Your Real Estate wallet is : {realEstate:N2} Euros.\n");
                        break;

                    case "2":
                        assetManagemetn.GetStockAsset(investorId, date);
                        break;
                    case "3":
                        assetManagemetn.GetFundAsset(investorId, date);
                        break;

                    case "4":
                        assetManagemetn.GetTotalAsset(investorId, date);
                        break; 

                    case "5":
                        InvestorProfileEnum profile = assetManagemetn.GetRiskProfile(investorId);
                        Console.WriteLine($"{profile} risk profile\n");
                        break;

                    case "0":
                        showMenu = false;
                        break;
                    default:
                        Console.WriteLine("Wrong option! Choose a number from menu.");
                        break;
                }
            }

            Console.WriteLine($"\n=== Enter a valid date and InvestmentId to view your portfolio size. (ex. 2025-07-20;Investor90 )=== \n");
            line = Console.ReadLine();
        }


    }

}