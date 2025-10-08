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

        private readonly AssetManagementService _assetManagementServiceTests;

        public AssetManagementServiceTests()
        {
            _stockService = new Mock<IStockService>();  

            _realEstateService = new Mock<IRealEstateService>();

            _fondService = new Mock<IFondService>();    

            _profileService = new Mock<IProfileService>();

            _assetManagementServiceTests = new AssetManagementService(_stockService.Object,
                _realEstateService.Object,
                _fondService.Object,
                _profileService.Object);


        }

        [Fact]
        public void GetFondAsset_ShouldReturnExpectedFondBalance()
        {

        }
    }
}