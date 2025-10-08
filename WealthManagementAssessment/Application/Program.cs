using Microsoft.Extensions.DependencyInjection;
using WealthManagementAssessment.Application;
using WealthManagementAssessment.Application.Models;
using WealthManagementAssessment.Application.Orchestration.Interfaces;
using WealthManagementAssessment.Domain.Enums;

class Program
{
    private static void Main(string[] args)
    {
        var host = Startup.CreateHost();

        var assetManagement = host.Services.GetRequiredService<IAssetManagementService>();

        string greeting = DateTime.Now.Hour < 12 ? "Good morning" : DateTime.Now.Hour < 18 ? "Good afternoon" : "Good evenning";

        Console.WriteLine($"\n                    === {greeting}, welcome to the Wealth Management Platform ===");
        Console.WriteLine($"\n=== Enter a valid date and InvestmentId to view your portfolio size. (ex. 2025-07-20;Investor90) === \n");

        var line = Console.ReadLine();

        while (!string.IsNullOrWhiteSpace(line))
        {
            var input = line.Split(";");

            var date = DateTime.Parse(input[0]);
            string investorId = input[1];

            bool showMenu = true;
            while (showMenu)
            {
                Console.WriteLine("Choose your investment type: \n");
                Console.WriteLine(" 1 - RealState Asset");
                Console.WriteLine(" 2 - Stock Asset");
                Console.WriteLine(" 3 - Fund Asset");
                Console.WriteLine(" 4 - Total Asset");
                Console.WriteLine(" 5 - Check your risk profile");
                Console.WriteLine(" 0 - Enter new InvestmentId and Date\n");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        InvestorBalanceResult rt = assetManagement.GetRealEstateAsset(investorId, date);
                        if (rt.RealStateBalance == 0)
                            Console.WriteLine("You don't have real estate investments for this period.\n");
                        else
                            Console.WriteLine($"Your Real Estate wallet is : {rt.RealStateBalance:N2} Euros.\n");
                        break;

                    case "2":
                        InvestorBalanceResult st = assetManagement.GetStockAsset(investorId, date);
                        if (st.StockBalance == 0)
                            Console.WriteLine("You don't have stock investments for this period.\n");
                        else
                            Console.WriteLine($"Your Stock wallet is : {st.StockBalance:N2} Euros.\n");
                        break;

                    case "3":
                        InvestorBalanceResult fn = assetManagement.GetFondAsset(investorId, date);
                        if (fn.FondBalance == 0)
                            Console.WriteLine("You don't have fond investments for this period.\n");
                        else
                            Console.WriteLine($"Your Fund wallet is : {fn.FondBalance:N2} Euros.\n");
                        break;

                    case "4":
                        InvestorBalanceResult asset = assetManagement.GetTotalAsset(investorId, date);
                        if (asset.TotalBalance == 0)
                            Console.WriteLine("You don't have investments for this period.\n");
                        else
                            Console.WriteLine($"Your total wallet is : {(asset.TotalBalance):N2} Euros.\n");
                        break;

                    case "5":
                        InvestorProfileEnum profile = assetManagement.GetRiskProfile(investorId);
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