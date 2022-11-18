using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AuthLecture.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(JwtAuthenticationManager jwtAuthenticationManager, ILogger<WeatherForecastController> logger)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            _logger = logger;
        }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] User usr)
        {
            var token = jwtAuthenticationManager.Authenticate(usr.username, usr.password);
            if(token==null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }

    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}