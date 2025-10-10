using Microsoft.Extensions.Options;
using Moq;
using WealthManagementAssessment.Application.Configuration;
using WealthManagementAssessment.Application.Orchestration;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;
using WealthManagementAssessment.Domain.Enums;
using static WealthManagementAssessment.Application.Configuration.AppConfig;

namespace WealthManagementAssessment.Tests
{
    public class AssetManagementServiceTests
    {
        private readonly Mock<IStockService> _stockService;

        private readonly Mock<IRealEstateService> _realEstateService;

        private readonly Mock<IFondService> _fondService;

        private readonly Mock<IProfileService> _profileService;

        private readonly Mock<IOptions<AppConfig>> _appConfig;

        private readonly AssetManagementService _assetManagementService;

        public AssetManagementServiceTests()
        {
            _stockService = new Mock<IStockService>();

            _realEstateService = new Mock<IRealEstateService>();

            _fondService = new Mock<IFondService>();

            _profileService = new Mock<IProfileService>();


            var testAppConfig = new AppConfig
            {
                RiskProfile = new RiskProfileLimit
                {
                    ConservativeUpperLimit = 1.33m,
                    ModerateUpperLimit = 1.66m
                }
            };

            _appConfig = new Mock<IOptions<AppConfig>>();

            _appConfig.SetupGet(x => x.Value).Returns(testAppConfig);

            _assetManagementService = new AssetManagementService(
                _stockService.Object,
                _realEstateService.Object,
                _fondService.Object,
                _profileService.Object,
                _appConfig.Object);
        }

        [Fact]
        public void GetRealEstate_ShouldReturnExpectedRealEstateBalance()
        {
            string ownerId = "investor90";
            DateTime valuationDate = new DateTime(2019, 12, 31);

            decimal expectedAmount = 10000m;

            _realEstateService
                .Setup(x => x.RealStateEngine(ownerId, valuationDate))
                .Returns(expectedAmount);

            var result = _assetManagementService.GetRealEstateAsset(ownerId, valuationDate);

            Assert.Equal(expectedAmount, result.RealStateBalance);
            Assert.Equal(expectedAmount, result.TotalBalance);
            Assert.Equal(0, result.StockBalance);
            Assert.Equal(0, result.FondBalance);
        }

        [Fact]
        public void GetFondAsset_ShouldReturnExpectedFondBalance()
        {
            string ownerId = "investor90";
            DateTime valuationDate = new DateTime(2019, 12, 31);
            decimal expectedAmount = 10000m;

            _fondService
                .Setup(x => x.FondEngine(ownerId, valuationDate))
                .Returns(expectedAmount);

            var result = _assetManagementService.GetFondAsset(ownerId, valuationDate);

            Assert.Equal(expectedAmount, result.FondBalance);
            Assert.Equal(expectedAmount, result.TotalBalance);
            Assert.Equal(0, result.StockBalance);
            Assert.Equal(0, result.RealStateBalance);
        }

        [Fact]

        public void GetStock_ShouldReturnExpectedStockBalance()
        {
            string ownerId = "Investor90";
            DateTime valuationDate = new DateTime(2019, 12, 31);

            decimal expetecedAmount = 10000m;

            _stockService
                .Setup(x => x.StockEngine(ownerId, valuationDate))
                .Returns(expetecedAmount);
            
            var result = _assetManagementService.GetStockAsset(ownerId, valuationDate);


            Assert.Equal(expetecedAmount, result.StockBalance);
            Assert.Equal(expetecedAmount, result.TotalBalance);
            Assert.Equal(0, result.RealStateBalance);
            Assert.Equal(0, result.FondBalance);
        }

        [Fact]
        public void GetTotalAsset_ShouldReturnExpectedTotalBalance()
        {
            string ownerId = "Investor90";
            DateTime valuationDate = new DateTime(2019, 12, 31);

            decimal expetecedRealEstateAmount = 10000m;
            decimal expetecedStockAmount = 20000m;
            decimal expetecedFondAmount = 30000m;

            _realEstateService.
                Setup(x => x.RealStateEngine(ownerId, valuationDate))
                .Returns(expetecedRealEstateAmount);

            _stockService
                .Setup(x => x.StockEngine(ownerId, valuationDate))
                .Returns(expetecedStockAmount);

            _fondService
                .Setup(x => x.FondEngine(ownerId, valuationDate))   
                .Returns(expetecedFondAmount);

            var result = _assetManagementService.GetTotalAsset(ownerId, valuationDate);

            Assert.Equal(expetecedStockAmount + expetecedFondAmount + expetecedRealEstateAmount, result.TotalBalance);
            Assert.Equal(expetecedStockAmount, result.StockBalance);
            Assert.Equal(expetecedFondAmount, result.FondBalance);
            Assert.Equal(expetecedRealEstateAmount, result.RealStateBalance);
        }

        [Fact]
        public void GetRiskProfile_ShouldReturnConservativetProfile()
        {
            string ownerId = "Investor90";
            decimal expetectedRiskProfile = 1.2m;

            _profileService.Setup(x => x.ProfileEngine(ownerId)).Returns(expetectedRiskProfile);

            var result = _assetManagementService.GetRiskProfile(ownerId);

            Assert.Equal(InvestorProfileEnum.Conservative, result);
        }

        [Fact]
        public void GetRiskProfile_ShouldReturnModerateProfile()
        {
            string ownerId = "investor90";
            decimal expetectedRiskProfile = 1.5m;

            _profileService.Setup(x => x.ProfileEngine(ownerId)).Returns(expetectedRiskProfile);

            var result = _assetManagementService.GetRiskProfile(ownerId);

            Assert.Equal(InvestorProfileEnum.Moderate, result);

        }

        [Fact]
        public void GetRiskProfile_ShouldReturnAggressiveeProfile()
        {
            string ownerId = "investor90";
            decimal expetectedRiskProfile = 1.7m;

            _profileService.Setup(x => x.ProfileEngine(ownerId)).Returns(expetectedRiskProfile);

            var result = _assetManagementService.GetRiskProfile(ownerId);

            Assert.Equal(InvestorProfileEnum.Aggressive, result);

        }

    }
}