using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Models.CountriesData;

namespace TestWebAPI.Repositories.CountryRepository
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryData>> Get();
        Task<IEnumerable<CountryData>> Get(string country);
        Task<CountryData> Create(CountryData countryData);
        Task Update(CountryData countryData);
        Task Delete(int id);
    }
}
