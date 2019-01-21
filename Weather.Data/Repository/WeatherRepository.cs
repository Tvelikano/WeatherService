using Dapper;

using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Weather.Data.Models;

namespace Weather.Data.Repository
{
    public class WeatherRepository : IRepository
    {
        private const string ConnectionString = "data source=VELIKANOVT;Initial Catalog=RecordsDataBase;Integrated Security=SSPI;";

        public async Task<ClientWeather> Get()
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();

                return await sqlConnection.QuerySingleOrDefaultAsync<ClientWeather>(
                    "SELECT * FROM dbo.Weather WHERE Id = (SELECT MAX(Id) FROM dbo.Weather)");
            }
        }

        public async Task Add(ClientWeather weather)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Temperature", weather.Temperature);
                dynamicParameters.Add("@Description", weather.Description);

                await sqlConnection.ExecuteAsync(
                    "addWeather",
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
