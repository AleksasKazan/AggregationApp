using Contracts.Models;

namespace Persistence.Repositories
{
    public interface ICsvRepository
    {
        Task<List<string>> DownloadCsvFilesAsync();
        List<string> RemoveOldCsvFile();
        List<ElectricityData> ParseCsv(List<string> csvFilePaths);
    }
}