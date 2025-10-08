using Moq;
using WealthManagementAssessment.Application.Orchestration;
using WealthManagementAssessment.Domain.Contracts.Interfaces;
using WealthManagementAssessment.Domain.Contracts.Services;

namespace WealthManagementAssessment.Tests
{
    public class AssetManagementServiceTests
    {
        private readonly Mock<IStockService> _stockService;

        private readonly Mock<IRealEstateService> _realEstateService;

        private readonly Mock<IFondService> _fondService;

        private readonly Mock<IProfileService> _profileService;

        private readonly AssetManagementService _assetManagementService;

        public AssetManagementServiceTests()
        {
            _stockService = new Mock<IStockService>();  

            _realEstateService = new Mock<IRealEstateService>();

            _fondService = new Mock<IFondService>();    

            _profileService = new Mock<IProfileService>();

            _assetManagementService = new AssetManagementService(_stockService.Object,
                _realEstateService.Object,
                _fondService.Object,
                _profileService.Object);


        }

        [Fact]
        public void GetFondAsset_ShouldReturnExpectedFondBalance()
        {
            string ownerId = "333";
            DateTime date = new DateTime(2019, 12, 20);
            decimal expectedAmount = 10000m;

            _fondService
                .Setup(x => x.FondEngine(ownerId,date) )
                .Returns(expectedAmount);   

            var result = _assetManagementService.GetFondAsset(ownerId, date);

            Assert.Equal(expectedAmount, result.FondBalance);
            Assert.Equal(0, result.StockBalance);
            Assert.Equal(0, result.RealStateBalance);
        }
    }
}