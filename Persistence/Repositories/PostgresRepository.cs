using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Contracts.Models;

namespace Persistence.Repositories
{
    public class PostgresRepository : IPostgresRepository
    {
        private readonly AggregationDbContext _dbContext;
        private readonly ILogger<PostgresRepository> _logger;

        public PostgresRepository(
            AggregationDbContext dbContext,
            ILogger<PostgresRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<AggregatedData>> GetAggregatedData()
        {
            try
            {
                var aggregatedData = await _dbContext.AggregatedData.ToListAsync();

                _logger.LogInformation("Aggregated data retrieved successfully.");

                return aggregatedData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting aggregated data.");
                return new List<AggregatedData>();
            }
        }

        public async Task SaveAggregatedData(List<AggregatedData> aggregatedData)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (aggregatedData.Any())
                        {
                            _dbContext.AggregatedData.RemoveRange(await _dbContext.AggregatedData.ToListAsync());
                            await _dbContext.SaveChangesAsync();

                            await _dbContext.AddRangeAsync(aggregatedData);
                            var result = await _dbContext.SaveChangesAsync();

                            transaction.Commit();

                            if (result > 0)
                            {
                                _logger.LogInformation("Aggregated data saved successfully. New records: " + result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error while saving Aggregated data.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving Aggregated data.");
            }
        }
    }
}
