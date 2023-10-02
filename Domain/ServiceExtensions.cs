using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICsvService, CsvService>()
                .AddScoped<IAggregationService, AggregationService>();
        }
    }
}