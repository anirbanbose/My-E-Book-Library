using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Common.DTO;

namespace MyEbookLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/weatherforecast")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IResult Get()
        {
            //var message = new EmailMessageDTO(["aspnetlancer@gmail.com", "anirbanbose2005@gmail.com"], "Test Email", "This is a test email");
            //_emailSender.SendEmail(message);
            //var zero = 0;

            //if(zero == 0)
            //{
            //    return Results.BadRequest();
            //}
            //_logger.LogInformation("This is an informational message.");
            //_logger.LogWarning("This is a warning message.");
            //_logger.LogError("This is an error message.");
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            return Results.Ok(result);
        }
    }
}
