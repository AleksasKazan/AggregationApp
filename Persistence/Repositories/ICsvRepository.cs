using Contracts.Models;

namespace Persistence.Repositories
{
    public interface ICsvRepository
    {
        Task<List<string>> DownloadCsvFiles();
        List<string> RemoveOldCsvFile();
        List<ElectricityData> ParseCsv(List<string> csvFilePaths);
    }
}