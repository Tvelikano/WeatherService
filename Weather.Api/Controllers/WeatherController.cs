using System.Web.Http;

using Weather.Data.Repository;

namespace Weather.Api.Controllers
{
    public class WeatherController : ApiController
    {
        private readonly IRepository _repository;

        public WeatherController()
        {
            _repository = new WeatherRepository();
        }

        public IHttpActionResult Get()
        {
            var weather = _repository.Get();

            return Ok(weather);
        }
    }
}
