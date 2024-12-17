using IniaAI.Server.Models;
using IniaAI.Server.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IEnumerable<DataPoint>> Get(string country, string subject)
        {
            var data = await _dataService.GetDataAsync(country, subject);
            return data;
        }
    }
}