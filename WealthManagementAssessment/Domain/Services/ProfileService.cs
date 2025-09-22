using System.Runtime.CompilerServices;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Repository;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Services
{
    public class ProfileService : IProfileService
    {

        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IFondService _fondService;

        private readonly IRealStateService _realStateService;

        private readonly IStockService _stockService;

        public ProfileService(IPortfolioRepository portfolioRepository, IFondService fondService, IRealStateService realStateService, IStockService stockService)
        {
            _portfolioRepository = portfolioRepository;
            _fondService = fondService;
            _realStateService = realStateService;
            _stockService = stockService;
        }

        public decimal ProfileEngine(string ownerId)
        {
            DateTime date = DateTime.Today;

            decimal investmentFond = _fondService.FondEngine(ownerId, date);
            decimal investmentStock = _stockService.StockEngine(ownerId, date);
            decimal investmentRealEstate = _realStateService.RealStateEngine(ownerId, date);

            decimal totalWallet = investmentFond + investmentStock + investmentRealEstate;

            //peso fond
            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = _portfolioRepository.GetAllInvestmentsByInvestor(ownerId, date)
                .Where(investment => investment.InvestmentType == "Fonds")
                .ToList();

            if (totalWallet == 0)
                return 0;

            if (fonds.Any())
            {
                decimal riskFond = CalculateAvareFondRisk(fonds);

                decimal riskProfile = (investmentRealEstate * 1 + investmentStock * 2 + investmentFond * riskFond) / totalWallet;

                return riskProfile;
            }


            return (investmentRealEstate * 1 + investmentStock * 2 ) / totalWallet; ;

        }

        private decimal CalculateAvareFondRisk(List<Investment> fonds)
        {
            DateTime date = DateTime.Today; 
            decimal totalRisk = 0;
            int count = 0;

            foreach (var fond in fonds)
            {
                decimal realEstate = _realStateService.RealStateEngine(fond.FondsInvestor, date);
                decimal stock = _stockService.StockEngine(fond.FondsInvestor, date);
                decimal totInvest = stock + realEstate;

                if (totInvest == 0)
                    continue;

                totalRisk = totalRisk + (realEstate * 1 + stock * 2) / totInvest;
                count++;
            }

            decimal fundRisk = count == 0 ? 0 : totalRisk / count;

            return fundRisk;
        }



        //APAGAR!!
        //public void TotalProfile()
        //{
        //    List<string> allInvestors = _portfolioRepository.GetAll();

        //    List<decimal> conservativeRisk = new List<decimal>();
        //    List<decimal> moderateRisk = new List<decimal>();
        //    List<decimal> agressiveRisk = new List<decimal>();

        //    foreach (var investor in allInvestors)
        //    {
        //        decimal risk = ProfileEngine(investor);


        //        if (risk < 1.33m)
        //            conservativeRisk.Add(risk);
        //        else if (risk > 1.33m && risk < 1.66m)
        //            moderateRisk.Add(risk);
        //        else
        //            agressiveRisk.Add(risk);

        //    }

        //    int n = conservativeRisk.Count + moderateRisk.Count + agressiveRisk.Count;
        //    Console.WriteLine($"tot conservative: {conservativeRisk.Count}");
        //    Console.WriteLine($"tot moderate: {moderateRisk.Count}");
        //    Console.WriteLine($"tot aggressive: {agressiveRisk.Count}");
        //}
    }
}
