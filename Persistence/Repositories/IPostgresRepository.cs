using Contracts.Models;

namespace Persistence.Repositories
{
    public interface IPostgresRepository
    {
        Task<List<AggregatedData>> GetAggregatedData();
        Task SaveAggregatedData(List<AggregatedData> aggregatedData);
    }
}
