using WealthManagementAssessment.WealthManagementService;

class Program
{

    private static void Main(string[] args)
    {
        string investorId = "Investor90";
        var obj = new AssetValuationService(investorId);

        Console.WriteLine("total investments: " + obj.TotalInvestments());
        Console.WriteLine("total Bulding Investor90: " + obj.RealStateEngine());

    }

}