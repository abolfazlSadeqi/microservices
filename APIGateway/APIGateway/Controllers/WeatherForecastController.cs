using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
   

    public WeatherForecastController()
    {
       
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult Get()
    {
        return  Ok();
    }
}