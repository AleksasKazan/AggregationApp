using Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AggregationDbContext : DbContext
    {
        public DbSet<AggregatedData> AggregatedData { get; set; }

        public AggregationDbContext(DbContextOptions<AggregationDbContext> options) : base(options)
        {
        }
    }
}
