using Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class CsvService : ICsvService
    {
        private readonly ILogger<CsvService> _logger;
        private readonly ICsvRepository _csvRepository;
        private readonly IPostgresRepository _postgresRepository;
        private readonly IAggregationService _aggregationService;

        public CsvService(
            ILogger<CsvService> logger, 
            ICsvRepository csvRepository,
            IPostgresRepository postgresRepository,
            IAggregationService aggregationService)
        {
            _logger = logger;
            _csvRepository = csvRepository;
            _postgresRepository = postgresRepository;
            _aggregationService = aggregationService;
        }

        public async Task SaveAggregatedData()
        {
            try
            {
                var csvFiles = await _csvRepository.DownloadCsvFiles();
                if (csvFiles.Any())
                {
                    csvFiles = _csvRepository.RemoveOldCsvFile();
                    var electricityData = _csvRepository.ParseCsv(csvFiles);
                    var aggregatedData = _aggregationService.AggregateElectricityData(electricityData);
                    await _postgresRepository.SaveAggregatedData(aggregatedData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing aggregated data.");
                throw;
            }
        }
    }
}
