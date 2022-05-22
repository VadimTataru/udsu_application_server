using TestWebAPI.Models.CovidData;

namespace TestWebAPI.Repositories.CovidDataRepository
{
    public interface ICovidDataRepository
    {
        Task<IEnumerable<CovidData>> Get();
        Task<IEnumerable<CovidData>> Get(string country);
        Task<IEnumerable<CovidData>> GetWithDate(string country, DateTime date_from, DateTime date_to);
        Task<List<CovidData>> Create(List<CovidData> data);
        Task Update(List<CovidData> data);
        Task Delete(int id);
    }
}
