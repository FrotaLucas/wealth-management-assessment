using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Application.Orchestration;
using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Infrastructure.Helper;
using WealthManagementAssessment.Infrastructure.Repository;

class Program
{
    private static void Main(string[] args)
    {

        string baseDir = AppContext.BaseDirectory;
        string projectDirectory = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent.FullName;


        string investorId = "Investor90";
        string dateString = "2028-12-31";


        //var line = Console.ReadLine();
        //var steps = line.Split(";");
        DateTime date = DateTime.Parse(dateString);

        Console.WriteLine("DateTime format: " + date);
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
                services.AddSingleton<IAssetRepository, AssetRepository>();
                services.AddSingleton<IAssetManagement, AssetManagementService>();
                services.AddSingleton<IPortfolioService, PortfolioService>();
                services.AddSingleton<IStockService, StockService>();
                services.AddSingleton<IRealStateService, RealStateService>();

            })
            .Build();


        var assetManagemetn = host.Services.GetRequiredService<IAssetManagement>();
        while(true)
        {

            Console.WriteLine(" 1 - RealState Engine");
            Console.WriteLine(" 2 - Stock Engine");
            Console.WriteLine(" 3 - FundEngine");

            string choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    assetManagemetn.GetRealEstateAsset(investorId, date);
                    break;
                case "2":
                    assetManagemetn.GetStockAsset(investorId, date);
                    break;
                case "3":
                    assetManagemetn.GetFundAsset(investorId, date);
                    break;
            }
        }



        //assetManagemetn.GetTotalAsset(investorId, date);
        //assetManagemetn.GetFundAsset(investorId, date);
        //assetManagemetn.GetStockAsset(investorId, date);
    }

}