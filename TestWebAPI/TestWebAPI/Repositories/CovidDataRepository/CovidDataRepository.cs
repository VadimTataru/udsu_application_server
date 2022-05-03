using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Database;
using TestWebAPI.Models.CovidData;

namespace TestWebAPI.Repositories.CovidDataRepository
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private readonly ApplicationContext _context;
        public CovidDataRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<CovidData> Create(CovidData data)
        {
            _context.CovidDatas.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task Delete(int id)
        {
            var dataToDelete = await _context.CovidDatas.FindAsync(id);
            _context.CovidDatas.Remove(dataToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CovidData>> Get()
        {
            return await _context.CovidDatas.ToListAsync();
        }

        public async Task<IEnumerable<CovidData>> Get(string country)
        {
            return await _context.CovidDatas.FromSqlRaw($"SELECT * FROM CovidDatas WHERE CovidDatas.CountryName = {country}").ToListAsync();
        }

        public async Task<IEnumerable<CovidData>> GetWithData(string data_from, string data_to)
        {
            return await _context.CovidDatas.FromSqlRaw($"SELECT * FROM CovidDatas WHERE CovidDatas.Date BETWEEN {data_from} AND {data_to} ").ToListAsync();
        }

        public async Task Update(CovidData data)
        {
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
