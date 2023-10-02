using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Repositories;
using System.Net;
using TestsData;

namespace AggregationApp.Controllers.Tests
{
    [TestClass()]
    public class ElectricityDataControllerTests
    {
        private readonly ElectricityDataController _controller;
        private readonly Mock<IPostgresRepository> _mockRepository;
        private readonly Mock<ILogger<ElectricityDataController>> _mockLogger;

        public ElectricityDataControllerTests()
        {
            _mockRepository = new Mock<IPostgresRepository>();
            _mockLogger = new Mock<ILogger<ElectricityDataController>>();

            _controller = new ElectricityDataController(_mockRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAggregatedData_ReturnsOkResult()
        {
            // Arrange
            var expectedData = TestData.AggregatedDataList;
            _mockRepository.Setup(repo => repo.GetAggregatedData()).ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetAggregatedData() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreSame(expectedData, result.Value);
        }

        [TestMethod]
        public async Task GetAggregatedData_ReturnsInternalServerError()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAggregatedData()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAggregatedData() as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result!.StatusCode);
        }
    }
}