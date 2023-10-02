using Contracts.Models;
using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Persistence.Repositories
{
    public class CsvRepository : ICsvRepository
    {
        private readonly ILogger<CsvRepository> _logger;
        private readonly string _baseUrl;
        private readonly string _datasetUrl;
        private const string DownloadsFolder = "Downloads";

        public CsvRepository(
            ILogger<CsvRepository> logger, 
            string baseUrl,
            string datasetUrl)
        {
            _logger = logger;
            _baseUrl = baseUrl;
            _datasetUrl = datasetUrl;
        }
  
        public List<string> RemoveOldCsvFile()
        {
            var allFiles = Directory.GetFiles(DownloadsFolder);
            if (allFiles.Length > 2)
            {
                string firstFile = allFiles.FirstOrDefault()!;
                File.Delete(firstFile);
            }
            
            return Directory.GetFiles(DownloadsFolder).ToList();
        }

        public async Task<List<string>> DownloadCsvFilesAsync()
        {
            using var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(_datasetUrl);

                if (response.IsSuccessStatusCode)
                {
                    string htmlContent = await response.Content.ReadAsStringAsync();
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlContent);

                    var csvLinks = htmlDocument.DocumentNode.SelectNodes("//a[contains(@class, 'button') and contains(text(), 'Atsisiųsti')]");

                    if (csvLinks != null)
                    {
                        var csvPaths = csvLinks.Select(link => link.GetAttributeValue("href", ""));

                        var filteredCsvPaths = csvPaths
                            .Where(ValidateCsvPath)
                            .TakeLast(2)
                            .ToList();

                        var csvFilesToDownload = await DownloadCsvFilesAsync(client, filteredCsvPaths);

                        return csvFilesToDownload;
                    }
                    else
                    {
                        _logger.LogWarning($"No CSV links found on the webpage.");
                    }
                }
                else
                {
                    _logger.LogWarning($"Failed to retrieve the webpage. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }

            return new List<string>();
        }      

        public List<ElectricityData> ParseCsv(List<string> csvFilePaths)
        {
            var electricityData = new List<ElectricityData>();
            foreach (var csvFilePath in csvFilePaths)
            {
                try
                {
                    using var reader = new StreamReader(csvFilePath);
                    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csv.Context.RegisterClassMap<ElectricityDataMap>();

                    if (ValidateCsvPath(csvFilePath))
                    {

                        electricityData.AddRange(
                            csv.GetRecords<ElectricityData>().ToList()
                        );
                    }
                    else
                    {
                        _logger.LogWarning("Invalid CSV file path.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error parsing CSV: {ex.Message}");
                    throw;
                }
            }
            return electricityData;
        }     

        private async Task<List<string>> DownloadCsvFilesAsync(HttpClient client, List<string> csvPaths)
        {
            var csvFilesToDownload = new List<string>();

            foreach (var csvPath in csvPaths)
            {
                var csvUrl = new Uri(new Uri(_baseUrl), csvPath).AbsoluteUri;
                string csvFilePath = Path.Combine(DownloadsFolder, Path.GetFileName(new Uri(csvPath).LocalPath));

                if (!File.Exists(csvFilePath))
                {
                    try
                    {
                        HttpResponseMessage csvResponse = await client.GetAsync(csvUrl);

                        if (csvResponse.IsSuccessStatusCode)
                        {
                            csvFilePath = await SaveCsvFile(csvResponse, csvFilePath);
                            csvFilesToDownload.Add(csvFilePath);
                        }
                        else
                        {
                            _logger.LogWarning($"Failed to download file. Status code: {csvResponse.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"An error occurred: {ex.Message}");
                    }
                }
            }

            return csvFilesToDownload;
        }

        private async Task<string> SaveCsvFile(HttpResponseMessage csvResponse, string csvFilePath)
        {
            try
            {
                if (!Directory.Exists(DownloadsFolder))
                {
                    Directory.CreateDirectory(DownloadsFolder);
                }

                using (Stream contentStream = await csvResponse.Content.ReadAsStreamAsync())
                using (FileStream fileStream = File.Create(csvFilePath))
                {
                    await contentStream.CopyToAsync(fileStream);
                }

                _logger.LogInformation($"File downloaded and saved to: {csvFilePath}");

                return csvFilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving the CSV file: {ex.Message}");
                return string.Empty;
            }
        }

        private static bool ValidateCsvPath(string csvPath)
        {
            var pathSegments = csvPath.Split('.');

            if (pathSegments.Length > 0)
            {
                var segments = pathSegments.First().Split('/');
                if (segments.Length > 0)
                {
                    string lastSegment = segments.Last();
                    bool isMatch = Regex.IsMatch(lastSegment, @"^\d{4}-\d{2}$");

                    return isMatch;
                }
            }

            return false;
        }
    }

    public sealed class ElectricityDataMap : ClassMap<ElectricityData>
    {
        public ElectricityDataMap()
        {
            Map(m => m.Tinklas).Name("TINKLAS");
            Map(m => m.ObtPavadinimas).Name("OBT_PAVADINIMAS");
            Map(m => m.ObjGvTipas).Name("OBJ_GV_TIPAS");
            Map(m => m.ObjNumeris).Name("OBJ_NUMERIS");
            Map(m => m.PPlus).Name("P+");
            Map(m => m.PlT).Name("PL_T");
            Map(m => m.PMinus).Name("P-");
        }
    }
}