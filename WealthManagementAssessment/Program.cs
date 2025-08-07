using WealthManagementAssessment.WealthManagementService;

class Program
{

    private static void Main(string[] args)
    {
        string investorId = "Investor90";
        string dateString = "2017-12-31";


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
        //var obj = new AssetValuation(investorId, date);
        //obj.FilesReader();

    }

}