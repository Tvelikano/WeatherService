using Newtonsoft.Json;

using System;
using System.IO;
using System.Web.Http;

using Weather.Api.Models;

namespace Weather.Api.Controllers
{
    public class WeatherController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (var reader = new StreamReader("weather.json"))
                {
                    var json = reader.ReadToEnd();
                    var weatherInfo = JsonConvert.DeserializeObject<ClientWeather>(json);

                    return Ok(weatherInfo);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
