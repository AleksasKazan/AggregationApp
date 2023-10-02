using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class AggregationDbContextFactory : IDesignTimeDbContextFactory<AggregationDbContext>
    {
        public AggregationDbContextFactory()
        {
        }

        private readonly IConfiguration Configuration;
        public AggregationDbContextFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public AggregationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AggregationDbContext>();
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostgresConnection"));

            return new AggregationDbContext(optionsBuilder.Options);
        }
    }
}
