using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WealthManagementAssessment.Application.Configuration;
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
                services.AddSingleton<IAssetManagement, AssetManagement>();

            })
            .Build();


        var assetManagemetn = host.Services.GetRequiredService<IAssetManagement>();

        //assetManagemetn.GetTotalAsset(investorId, date);
        assetManagemetn.GetFundAsset(investorId, date);
        //assetManagemetn.GetStockAsset(investorId, date);
    }

}