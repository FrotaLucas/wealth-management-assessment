using WealthManagementAssessment.WealthManagementService;

class Program
{

    private static void Main(string[] args)
    {
        string investorId = "Investor90";

        //var line = Console.ReadLine();
        //var steps = line.Split(";");
        //DateTime date = DateTime.Parse(steps[1]);

        //Console.WriteLine("date format"+ date);
        //12/28/2015 12:00:00 AM

        var obj = new AssetValuationService(investorId);

        Console.WriteLine("total investments: " + obj.TotalInvestments());
        Console.WriteLine("total Bulding Investor90: " + obj.RealStateEngine());

    }

}