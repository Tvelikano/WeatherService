using Microsoft.Owin.Hosting;

using Newtonsoft.Json;

using System;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using Weather.Data.Models;
using Weather.Data.Repository;
using Weather.Service.Models;

namespace Weather.Service
{
    public partial class WeatherService : ServiceBase
    {
        private const string BaseAddress = "http://localhost:9000/";
        private IDisposable _server;

        private const string ApiUrl = "https://api.openweathermap.org/data/2.5/weather";
        private const string CityId = "625665";
        private const string AppId = "49fcd289ff522b35ddb23de3fd7708a5";
        private const string Units = "metric";
        private const string Language = "ru";
        private const string Mode = "";

        private readonly Timer _timer;
        private const int Interval = 5400000; // 1,5 Hours 

        private readonly IRepository _repository;

        public WeatherService()
        {
            InitializeComponent();
            _repository = new WeatherRepository();

            Task.Run(GetWeather).Wait();

            _timer = new Timer(Interval);
            _timer.Elapsed += (sender, e) => Task.Run(GetWeather).Wait();

            _timer.Enabled = true;
        }

        protected override void OnStart(string[] args)
        {
            _server = WebApp.Start<Startup>(url: BaseAddress);
        }

        protected override void OnStop()
        {
            _timer?.Dispose();
            _server?.Dispose();
            base.OnStop();
        }

        private async Task GetWeather()
        {
            var requestUrl = $"{ApiUrl}?id={CityId}&APPID={AppId}&units={Units}&lang={Language}&mode={Mode}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                var responseMessage = await client.GetAsync(requestUrl);

                var weather = JsonConvert.DeserializeObject<WeatherInfo>(await responseMessage.Content.ReadAsStringAsync());

                var clientWeather = new ClientWeather
                {
                    Temperature = weather.Main.Temp,
                    Description = weather.Weather.FirstOrDefault()?.Description
                };

                await _repository.Add(clientWeather);
            }
        }
    }
}
