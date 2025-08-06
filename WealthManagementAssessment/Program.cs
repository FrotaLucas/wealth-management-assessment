using WealthManagementAssessment.WealthManagementService;

class Program
{

    private static void Main(string[] args)
    {
        string investorId = "Investor90";
        string dateString = "06-28-2016";

        //var line = Console.ReadLine();
        //var steps = line.Split(";");
        DateTime date = DateTime.Parse(dateString);

        Console.WriteLine("DateTime format: " + date);
        //1/16/2016 12:00:00 AM

        var obj = new AssetValuationService(investorId, date);

        Console.WriteLine("total investments: " + obj.TotalInvestments());
        obj.RealStateEngine();

        Console.WriteLine("Valuation RealState Bulding + ESTATE: " + obj.RealStateSumup);
        Console.WriteLine("Valuation Stocks: " + obj.StockSumup);
        

    }

}