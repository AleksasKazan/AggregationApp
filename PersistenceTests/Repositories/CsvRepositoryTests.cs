using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Persistence.Repositories.Tests
{
    [TestClass()]
    public class CsvRepositoryTests
    {
        private CsvRepository _csvRepository;
        private Mock<ILogger<CsvRepository>> _loggerMock;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IConfiguration> _configurationMock;
        private const string DownloadsFolder = "Downloads";

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerMock = new Mock<ILogger<CsvRepository>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _csvRepository = new CsvRepository(_loggerMock.Object, _httpClientFactoryMock.Object, _configurationMock.Object);
        }

        [TestMethod]
        public void RemoveOldCsvFile_Success()
        {
            // Arrange
            const string csvFileName1 = "2022-03.csv";
            const string csvFileName2 = "2022-04.csv";
            const string csvFileName3 = "2022-05.csv";
            Directory.CreateDirectory(DownloadsFolder);
            File.Create(Path.Combine(DownloadsFolder, csvFileName1)).Close();
            File.Create(Path.Combine(DownloadsFolder, csvFileName2)).Close();
            File.Create(Path.Combine(DownloadsFolder, csvFileName3)).Close();

            // Act
            var result = _csvRepository.RemoveOldCsvFile();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(filePath => !File.Exists(Path.Combine(DownloadsFolder, csvFileName1))));
        }
    }
}