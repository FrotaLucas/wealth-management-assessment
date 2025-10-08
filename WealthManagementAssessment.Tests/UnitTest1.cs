using Moq;
using WealthManagementAssessment.Domain.Contracts.Interfaces;

namespace WealthManagementAssessment.Tests
{
    public class AssetManagementServiceTest
    {
        private readonly Mock<IStockService> _stockService;

        private readonly Mock<IRealEstateService> _realStateService;

        [Fact]
        public void GetFondAsset_ShouldReturnExpectedFondBalance()
        {

        }
    }
}