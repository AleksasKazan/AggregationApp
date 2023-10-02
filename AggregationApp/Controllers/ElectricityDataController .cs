using Microsoft.AspNetCore.Mvc;
using System.Net;
using Persistence.Repositories;

namespace AggregationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ElectricityDataController : ControllerBase
    {
        private readonly IPostgresRepository _postgresRepository;
        private readonly ILogger<ElectricityDataController> _logger;

        public ElectricityDataController(
            IPostgresRepository postgresRepository, 
            ILogger<ElectricityDataController> logger)
        {
            _postgresRepository = postgresRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAggregatedData()
        {
            try
            {
                var aggregatedData = await _postgresRepository.GetAggregatedData();

                _logger.LogInformation("Aggregated data retrieved successfully.");

                return Ok(aggregatedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting aggregated data.");

                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing your request.",
                };

                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}