using Contracts.Models;

namespace Domain.Services
{
    public interface IAggregationService
    {
        List<AggregatedData> AggregateElectricityData(List<ElectricityData> electricityData);
    }
}