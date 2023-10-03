using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;
using Moq;
using TestsData;

namespace Domain.Services.Tests
{
    [TestClass()]
    public class CsvServiceTests
    {
        private readonly CsvService _csvService;
        private readonly Mock<ILogger<CsvService>> _loggerMock;
        private readonly Mock<ICsvRepository> _csvRepositoryMock;
        private readonly Mock<IPostgresRepository> _postgresRepositoryMock;
        private readonly Mock<IAggregationService> _aggregationServiceMock;
        public CsvServiceTests()
        {
            _loggerMock = new Mock<ILogger<CsvService>>();
            _csvRepositoryMock = new Mock<ICsvRepository>();
            _postgresRepositoryMock = new Mock<IPostgresRepository>();
            _aggregationServiceMock = new Mock<IAggregationService>();

            _csvService = new CsvService(
                _loggerMock.Object,
                _csvRepositoryMock.Object,
                _postgresRepositoryMock.Object,
                _aggregationServiceMock.Object);
        }

        [TestMethod]
        public async Task SaveAggregatedData_Success()
        {
            // Arrange
            var csvFiles = new List<string> { "Downloads/2022-04.csv", "Downloads/2022-05.csv" };
            var electricityData = TestData.ElectricityDataList;

            var aggregatedData = TestData.AggregatedDataList;

            _csvRepositoryMock.Setup(repo => repo.DownloadCsvFiles())
                .ReturnsAsync(csvFiles);
            _csvRepositoryMock.Setup(repo => repo.RemoveOldCsvFile())
                .Returns(csvFiles);
            _csvRepositoryMock.Setup(repo => repo.ParseCsv(csvFiles))
                .Returns(electricityData);
            _aggregationServiceMock.Setup(service => service.AggregateElectricityData(electricityData))
                .Returns(aggregatedData);

            // Act
            await _csvService.SaveAggregatedData();

            // Assert
            _postgresRepositoryMock.Verify(repo => repo.SaveAggregatedData(aggregatedData), Times.Once);
        }

        [TestMethod]
        public async Task SaveAggregatedData_Exception()
        {
            // Arrange
            _csvRepositoryMock.Setup(repo => repo.DownloadCsvFiles())
                .ThrowsAsync(new Exception("Test exception"));

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _csvService.SaveAggregatedData());
        }
    }
}