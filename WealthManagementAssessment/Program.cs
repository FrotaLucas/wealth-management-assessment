using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Infrastructure.Repository;

class Program
{
    private static void Main(string[] args)
    {

        string baseDir = AppContext.BaseDirectory;
        string projectDirectory = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent.FullName;


        string investorId = "Investor90";
        string dateString = "2018-12-31";


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
                cfg.AddJsonFile(Path.Combine("Application", $"appsetings.{ctx.HostingEnvironment.EnvironmentName}.json"), optional: true, reloadOnChange: true);
                
                cfg.AddEnvironmentVariables();

            })
            .ConfigureServices( (ctx, services) =>
            {
                services.Configure<AppConfig>(options =>
                {
                    ctx.Configuration.Bind(options);

                    options.CsvsPaths.Investments = Path.Combine(projectDirectory, options.CsvsPaths.Investments);
                    options.CsvsPaths.Transactions = Path.Combine( projectDirectory, options.CsvsPaths.Transactions );
                    options.CsvsPaths.Quotes = Path.Combine(projectDirectory, options.CsvsPaths.Quotes);

                });

                services.AddSingleton<IFilesReader, FilesReader>();
                services.AddSingleton<IAssetRepository, AssetRepository>();
                services.AddSingleton<IAssetManagement, AssetManagement>();

            })
            .Build();


        var assetManagemetn = host.Services.GetRequiredService<IAssetManagement>();

        assetManagemetn.DisplayAsset(investorId, date);

        //Sol1
        //var obj = new AssetValuationService(investorId, date);

        //Console.WriteLine("total investments: " + obj.TotalInvestments());
        //obj.RealStateEngine();

        //Console.WriteLine("Valuation RealState Bulding + ESTATE: " + obj.RealStateSumup);
        //Console.WriteLine("Valuation Stocks: " + obj.StockSumup);

        //Sol2
        //var obj = new AssetRepository(investorId, date);
        //obj.FilesReader();

        //obj.AssetEngine();


        //sol 3

        //void AssetEngine();
        //IFilesReader filesReader = new FilesReader();

        //IAssetRepository interfaceRepository = new AssetRepository(filesReader);

        ////precisa LER antes os files dentro de repository
        //AssetManagement asset = new AssetManagement(interfaceRepository);

        //asset.DisplayAsset(investorId , date);
    }

}