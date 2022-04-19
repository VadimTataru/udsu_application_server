using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebAPI.Database;
using TestWebAPI.Models;
using TestWebAPI.Models.CountriesData;

namespace TestWebAPI.Repositories.CountryRepository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationContext _context;
        public CountryRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<CountryData> Create(CountryData countryData)
        {
            _context.Countries.Add(countryData);
            await _context.SaveChangesAsync();
            return countryData;
        }

        public async Task Delete(int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(countryToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CountryData>> Get()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<IEnumerable<CountryData>> Get(string countryName)
        {
            return await _context.Countries.FromSqlInterpolated($"SELECT * FROM Countries WHERE Countries.CountryName = {countryName}").ToListAsync();
            //return await _context.Countries.FindAsync(countryName);
        }

        public async Task Update(CountryData countryData)
        {
            _context.Entry(countryData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
