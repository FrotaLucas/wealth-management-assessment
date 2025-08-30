using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Infrastructure.Repository;

class Program
{

    private static void Main(string[] args)
    {
        string investorId = "Investor90";
        string dateString = "2018-12-31";


        //var line = Console.ReadLine();
        //var steps = line.Split(";");
        DateTime date = DateTime.Parse(dateString);

        Console.WriteLine("DateTime format: " + date);
        //1/16/2016 12:00:00 AM


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

        IFilesReader filesReader = new FilesReader();

        IAssetRepository interfaceRepository = new AssetRepository(investorId, date, filesReader);

        //precisa LER antes os files dentro de repository
        AssetManagement asset = new AssetManagement(interfaceRepository);

        asset.DisplayAsset();
    }

}