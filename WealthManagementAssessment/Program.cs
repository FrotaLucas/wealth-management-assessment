using WealthManagementAssessment.WealthManagementService;

class Program
{

    private static void Main(string[] args)
    {
        string investorId = "Investor90";
        string dateString = "04-26-2020";

        //var line = Console.ReadLine();
        //var steps = line.Split(";");
        DateTime date = DateTime.Parse(dateString);

        //Console.WriteLine("date format" + date);
        //8/4/2025 12:00:00 AM

        var obj = new AssetValuationService(investorId, date);

        Console.WriteLine("total investments: " + obj.TotalInvestments());
        Console.WriteLine("total Bulding Investor90: " + obj.RealStateEngine());

    }

}