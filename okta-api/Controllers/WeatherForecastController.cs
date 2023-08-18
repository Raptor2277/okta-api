using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using okta_api.Repositories;
using okta_api.Services;
using System.ComponentModel.DataAnnotations;

namespace okta_api.Controllers
{
    [ApiController]
    [Route("/api")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IExtendedConfiguration config)
        {
            _logger = logger;
        }

        [HttpGet("/api/v1/testRepo")]
        public async Task<string> Get([FromServices] TestRepo repo)
        {
            string data = await repo.GetApps();

            return data;
        }

        [HttpGet("/api/v1/testRepo")]
        public async Task<string> Ge2t([FromServices] TestRepo repo)
        {
            string data = await repo.GetApps();

            return data;
        }

        [HttpGet("/api/v1/testRepo")]
        public async Task<string> Get2([FromServices] TestRepo repo)
        {
            string data = await repo.GetApps();

            return data;
        }
    }
}