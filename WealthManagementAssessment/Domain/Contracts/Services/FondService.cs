using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Entities;

namespace WealthManagementAssessment.Domain.Contracts.Services
{
    public class FondService : IFondService
    {
        private readonly IAssetRepository _assetRepository;

        private readonly IRealStateService _realStateService;

        private readonly IStockService _stockService;
        
        public FondService(IAssetRepository assetRepository, IRealStateService realStateService, IStockService stockService)
        {
            _assetRepository = assetRepository;
            _realStateService = realStateService;
            _stockService = stockService;
        }



        public double FondEngine(string ownerId, DateTime valuationDate)
        {
            double fondSumup = 0;

            //USAR ENUM ao inves de STRING!!!!!!!!
            List<Investment> fonds = _assetRepository.GetAllInvestmentsByInvestor(ownerId, valuationDate)
                .Where(investment => investment.InvestmentType == "Fonds")
                .ToList();

            //_filesReader.ReadTransactions(fonds, valuationDate);

            //old code
            //List<Investment> allInvestments = _filesReader.ReadInvestments();


            //new code
            //Dictionary<string, List<Investment>> dictionary = _filesReader.GetDictionary(ownerId, valuationDate);
            Dictionary<string, List<Investment>> dictionary = _assetRepository.GetAllInvestmentsByFonds(ownerId, valuationDate);



            foreach (var fond in fonds)
            {
                double totalPercentage = fond.Transactions.Sum(t => t.Value);

                //old code
                //List<Investment> investmentsOfFound = allInvestments.Where(i => i.InvestorId == fond.FondsInvestor).ToList();
                //_filesReader.ReadTransactions(investmentsOfFound, valuationDate);

                //new code
                dictionary.TryGetValue(fond.FondsInvestor, out var investmentsOfFound);


                double realStateSumup = _realStateService.RealStateEngine(investmentsOfFound);

                double stockSumup = _stockService.StockEngine(investmentsOfFound, valuationDate);


                fondSumup = fondSumup + totalPercentage * (realStateSumup + stockSumup);
            }

            return fondSumup;
        }
    }
}
