using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contracts.Models;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using TestsData;

namespace Domain.Services.Tests
{
    [TestClass()]
    public class AggregationServiceTests
    {
        private readonly AggregationService _aggregationService;
        private readonly Mock<ILogger<AggregationService>> _loggerMock;

        public AggregationServiceTests()
        {
            _loggerMock = new Mock<ILogger<AggregationService>>();
            _aggregationService = new AggregationService(_loggerMock.Object);
        }

        [TestMethod]
        public void AggregateElectricityData_Success()
        {
            // Arrange
            var electricityData = TestData.ElectricityDataList;

            var expectedAggregatedData = TestData.AggregatedDataList;

            // Act
            var aggregatedData = _aggregationService.AggregateElectricityData(electricityData);

            // Assert
            aggregatedData.Should().BeEquivalentTo(expectedAggregatedData);
        }

        [TestMethod]
        public void AggregateElectricityData_Exception()
        {
            // Arrange
            List<ElectricityData> electricityData = null;

            // Act and Assert
            Assert.ThrowsException<ArgumentNullException>(() => _aggregationService.AggregateElectricityData(electricityData));
        }
    }
}