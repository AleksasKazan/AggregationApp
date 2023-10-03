using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestsData;

namespace Persistence.Repositories.Tests
{
    [TestClass()]
    public class PostgresRepositoryTests
    {
        private readonly Mock<ILogger<PostgresRepository>> _loggerMock;
        private readonly PostgresRepository _repository;
        private readonly AggregationDbContext _dbContext;

        public PostgresRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AggregationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AggregationDbContext(dbContextOptions);

            _loggerMock = new Mock<ILogger<PostgresRepository>>();
            _repository = new PostgresRepository(_dbContext, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetAggregatedData_Success()
        {
            // Arrange
            _dbContext.AggregatedData.AddRange(TestData.AggregatedDataList);
            _dbContext.SaveChanges();

            // Act
            var result = await _repository.GetAggregatedData();

            // Assert
            Assert.IsNotNull(result);
        }      
    }
}