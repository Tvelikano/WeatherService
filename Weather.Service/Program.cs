using System.ServiceProcess;

namespace Weather.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new WeatherService()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
