using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                .AddSingleton<ICsvRepository>(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<CsvRepository>>();
                    var baseUrl = configuration["ElectricityDataSource:BaseUrl"];
                    var datasetUrl = baseUrl + configuration["ElectricityDataSource:DatasetUrl"];

                    return new CsvRepository(logger, baseUrl!, datasetUrl);
                })
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