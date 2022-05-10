using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TestWebAPI.Database;
using TestWebAPI.Models.CovidData;
using TestWebAPI.ThirdPartyCoivdAPI;
using Newtonsoft.Json;

namespace TestWebAPI.Repositories.CovidDataRepository
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private readonly ApplicationContext _context;
        public CovidDataRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<CovidData>> Create(List<CovidData> data)
        {
            foreach(var d in data) _context.CovidDatas.Add(d);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task Delete(int id)
        {
            var dataToDelete = await _context.CovidDatas.FindAsync(id);
            if (dataToDelete != null)
            {
                _context.CovidDatas.Remove(dataToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CovidData>> Get()
        {
            return await _context.CovidDatas.ToListAsync();
        }

        public async Task<IEnumerable<CovidData>> Get(string country)
        {
            return await _context.CovidDatas.FromSqlRaw($"SELECT * FROM CovidDatas WHERE CovidDatas.CountryName = {country}").ToListAsync();
        }

        public async Task<IEnumerable<CovidData>> GetWithDate(string country, DateTime date_from, DateTime date_to)
        {
            int dateDuration = (int)(date_to - date_from).TotalDays + 1;
            List<CovidData> dataFromDB = await _context.CovidDatas.FromSqlRaw($"SELECT * FROM CovidDatas").Where(c => c.CountryName == country).Where(c => c.Date >= date_from).Where(c => c.Date <= date_to).OrderBy(c => c.Date).ToListAsync();

            if (dateDuration > dataFromDB.Count)
            {
                string from = date_from.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string to = date_to.ToString("yyyy-MM-ddTHH:mm:ssZ");
                List<CovidData>? result_data = CallGetRequest(country, from, to);

                if (dataFromDB.Count > 0)
                {
                    result_data = result_data?.Except(dataFromDB).ToList();
                }

                if(result_data?.Count > 0)
                {
                    await Create(result_data);
                    return await GetWithDate(country, date_from, date_to);
                }                
            }
            return dataFromDB;
        }

        public async Task Update(CovidData data)
        {
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private List<CovidData>? CallGetRequest(string country, string from, string to)
        {
            var request = new GetRequest($"https://api.covid19api.com/country/{country}?from={from}&to={to}");
            request.Run();
            var response = request.responseData;
            if (response != null)
            {
                var json = JArray.Parse(response);
                List<CovidData>? data = JsonConvert.DeserializeObject<List<CovidData>>(json.ToString());
                return data;
            }
            return null;
        }

        private List<CovidData> GetResultList(List<CovidData> fromThirdAPI, List<CovidData> fromDB)
        {
            List<CovidData> resultList = fromThirdAPI.Except(fromDB).ToList();
            return resultList;
        }
    }
}
