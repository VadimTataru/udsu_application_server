using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Models.CovidData;

namespace TestWebAPI.Repositories.CovidDataRepository
{
    public interface ICovidDataRepository
    {
        Task<IEnumerable<CovidData>> Get();
        Task<IEnumerable<CovidData>> Get(string country);
        Task<IEnumerable<CovidData>> GetWithData(string data_from, string data_to);
        Task<CovidData> Create(CovidData data);
        Task Update(CovidData data);
        Task Delete(int id);
    }
}
