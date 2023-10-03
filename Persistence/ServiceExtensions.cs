using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddRepositories(configuration);
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IPostgresRepository, PostgresRepository>()
                .AddScoped<ICsvRepository, CsvRepository>()
                .AddDbContext<AggregationDbContext>(options =>
                {
                    options.UseNpgsql(
                        configuration.GetConnectionString("PostgresConnection"));
                }, ServiceLifetime.Scoped)
                .AddDatabaseMigrations();
        }

        private static IServiceCollection AddDatabaseMigrations(this IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AggregationDbContext>();
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }

            return services;
        }
    }
}