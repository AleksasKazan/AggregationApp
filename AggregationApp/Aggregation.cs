using Domain.Services;

namespace AggregationApp
{
    public class Aggregation : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Aggregation> _logger;

        public Aggregation(
            IServiceProvider serviceProvider, 
            ILogger<Aggregation> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aggregation is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scopedICsvService = scope.ServiceProvider.GetRequiredService<ICsvService>();
                        await scopedICsvService.SaveAggregatedData();

                        _logger.LogInformation("Aggregated data saved successfully.");
                    }
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing CSV files.");
                }
            }

            _logger.LogInformation("Aggregation is stopping.");
        }
    }
}
