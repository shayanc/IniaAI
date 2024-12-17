using IniaAI.Server.Models;
using IniaAI.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IniaAI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Retrieves country-specific data based on the provided subject.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get country-specific data based on subject",
            Description = "Fetches data points for a specific country and subject/category (e.g., GDP, population)."
        )]
        [SwaggerResponse(200, "Request succeeded. Returns a collection of data points.", typeof(IEnumerable<DataPoint>))]
        [SwaggerResponse(400, "Bad Request. The input parameters (country or subject) are invalid or missing.")]
        [SwaggerResponse(404, "Not Found. No data was found for the specified country and subject.")]
        [SwaggerResponse(500, "Internal Server Error. An unexpected error occurred on the server.")]
        public async Task<IActionResult> Get(string country, string subject)
        {
            // Check for missing parameters and return 400 Bad Request
            if (string.IsNullOrWhiteSpace(country))
            {
                return BadRequest("The 'country' parameter is required.");
            }
            if (string.IsNullOrWhiteSpace(subject))
            {
                return BadRequest("The 'subject' parameter is required.");
            }

            // Fetch data from the service
            var data = await _dataService.GetDataAsync(country, subject);

            // Return 404 if no data is found
            if (data == null || !data.Any())
            {
                return NotFound("No data found for the specified country and subject.");
            }

            // Return 200 OK with the data
            return Ok(data);
        }
    }
}