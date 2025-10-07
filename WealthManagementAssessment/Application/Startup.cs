using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Application.Orchestration;
using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Services;
using WealthManagementAssessment.Infrastructure.Helper;
using WealthManagementAssessment.Infrastructure.Repository;

namespace WealthManagementAssessment.Application
{
    public class Startup
    {
        public static IHost NewMethodd()
        {
            string baseDir = AppContext.BaseDirectory;

            string projectDirectory = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent.FullName;


            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.SetBasePath(AppContext.BaseDirectory);

                    cfg.AddJsonFile(Path.Combine("Application", "appsettings.json"), optional: true, reloadOnChange: true);
                    cfg.AddJsonFile(Path.Combine("Application", $"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json"), optional: true, reloadOnChange: true);

                    cfg.AddEnvironmentVariables();

                })
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<AppConfig>(options =>
                    {
                        ctx.Configuration.Bind(options);

                        options.CsvPath.Investments = Path.Combine(projectDirectory, options.CsvPath.Investments);
                        options.CsvPath.Transactions = Path.Combine(projectDirectory, options.CsvPath.Transactions);
                        options.CsvPath.Quotes = Path.Combine(projectDirectory, options.CsvPath.Quotes);

                    });

                    services.AddSingleton<IDataSource, DataSource>();
                    services.AddSingleton<IPortfolioRepository, PortfolioRepository>();
                    services.AddSingleton<IAssetManagementService, AssetManagementService>();
                    services.AddSingleton<IStockService, StockService>();
                    services.AddSingleton<IRealStateService, RealEstateService>();
                    services.AddSingleton<IProfileService, ProfileService>();
                    services.AddSingleton<IFondService, FondService>();

                })
                .Build();

            return host;
        }
    }
}
