using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestWebAPI.Database;
using TestWebAPI.Models.CountriesData;
using TestWebAPI.ThirdPartyCoivdAPI;

namespace TestWebAPI.Repositories.CountryRepository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationContext _context;

        public CountryRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<CountryData>> Create(List<CountryData> countryData)
        {
            foreach(var country in countryData) _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return countryData;
        }

        public async Task Delete(int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);
            if (countryToDelete != null)
            {
                _context.Countries.Remove(countryToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CountryData>> Get()
        {
            List<CountryData> data = await _context.Countries.FromSqlRaw($"SELECT * FROM Countries").OrderBy(c=>c.CountryName).ToListAsync();
            if (!data.Any())
            {
                //запрос к сторонней апи
                var request = new GetRequest("https://api.covid19api.com/countries");
                request.Run();
                var response = request.responseData;

                if (response != null)
                {
                    var json = JArray.Parse(response);
                    List<CountryData>? countryList = JsonConvert.DeserializeObject<List<CountryData>>(json.ToString());

                    if (countryList?.Count > 0)
                    {
                        await Create(countryList);
                        await Get();
                    }
                }
            }
            return await _context.Countries.ToListAsync();
        }

        public async Task Update(CountryData countryData)
        {
            _context.Entry(countryData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
