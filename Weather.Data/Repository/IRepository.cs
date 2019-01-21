using System.Threading.Tasks;

using Weather.Data.Models;

namespace Weather.Data.Repository
{
    public interface IRepository
    {
        Task<ClientWeather> Get();

        Task Add(ClientWeather weather);
    }
}
