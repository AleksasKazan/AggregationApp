using Contracts.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class AggregationService : IAggregationService
    {
        private const string _filteredOn = "Butas";
        private readonly ILogger<AggregationService> _logger;

        public AggregationService(ILogger<AggregationService> logger)
        {
            _logger = logger;
        }
        public List<AggregatedData> AggregateElectricityData(List<ElectricityData> electricityData)
        {
            try
            {
                var aggregatedData = electricityData
                   .Where(e => e.ObtPavadinimas == _filteredOn)
                   .GroupBy(e => e.Tinklas)
                   .Select(group => new AggregatedData
                   {
                       Tinklas = group.Key,
                       TotalRecords = group.Count(),
                       PMinusSum = group.Sum(e => e.PMinus),
                       PPlusSum = group.Sum(e => e.PPlus),
                   })
                   .ToList();

                return aggregatedData;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"Error processing CSV data: {ex.Message}");
                throw;
            }
        }
    }
}
