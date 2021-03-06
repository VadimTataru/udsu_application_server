using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TestWebAPI.Database;
using TestWebAPI.Models.CovidData;
using TestWebAPI.ThirdPartyCoivdAPI;
using Newtonsoft.Json;
using TestWebAPI.Models;
using TestWebAPI.Models.CountriesData;

namespace TestWebAPI.Repositories.CovidDataRepository
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private readonly ApplicationContext _context;
        public CovidDataRepository(ApplicationContext context)
        {
            _context = context;
        }

        private async Task<List<HistoricalCovidData>> CreateHistorical(List<HistoricalCovidData> data)
        {
            foreach (var d in data) _context.HistoricalCovidDatas.Add(d);
            await _context.SaveChangesAsync();
            return data;
        }
        public async Task<List<CovidData>> Create(List<CovidData> data)
        {
            ModifyZeroData(data);
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
            //List<CountryData> countryData = await _context.Countries.FromSqlRaw($"SELECT Countries.ISO2 FROM Countries").Where(c => c.CountryName == country).ToListAsync();
            List<CovidData> dataFromDB = await _context.CovidDatas.FromSqlRaw($"SELECT * FROM CovidDatas").Where(c => c.CountryName == country).Where(c => c.Date >= date_from).Where(c => c.Date <= date_to).OrderBy(c => c.Date).ToListAsync();
            List<HistoricalCovidData> hisDataFromDB = await _context.HistoricalCovidDatas.FromSqlRaw($"SELECT * FROM CovidDatas").Where(c => c.CountryName == country).Where(c => c.Date >= date_from).Where(c => c.Date <= date_to).OrderBy(c => c.Date).ToListAsync();            
            if (dateDuration > dataFromDB.Count)
            {
                string from = date_from.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string to = date_to.ToString("yyyy-MM-ddTHH:mm:ssZ");
                List<CovidData>? result_data = new List<CovidData>();
                List<HistoricalCovidData>? historicalCovidDatas = new List<HistoricalCovidData>();
                var json = await CallGetRequest(country, from, to);

                if (json != null)
                {
                    result_data = JsonConvert.DeserializeObject<List<CovidData>>(json.ToString());
                    historicalCovidDatas = JsonConvert.DeserializeObject<List<HistoricalCovidData>>(json.ToString());
                }                    

                if (dataFromDB.Count > 0)
                {
                    result_data = result_data?.Except(dataFromDB).ToList();
                    historicalCovidDatas = historicalCovidDatas?.Except(hisDataFromDB).ToList();
                }

                if(result_data?.Count > 0)
                {
                    await CreateHistorical(historicalCovidDatas);
                    await Create(result_data);

                    return await GetWithDate(country, date_from, date_to);
                }                
            }

            if ((dataFromDB.Min(d => d.Recovered) == 0 || dataFromDB.Min(d => d.Confirm) == 0 || dataFromDB.Min(d => d.Deaths) == 0)
                            && dataFromDB.Last().Recovered == 0)
                dataFromDB = await Update(dataFromDB);

            return dataFromDB;
        }

        public async Task<List<CovidData>> Update(List<CovidData> data)
        {
            ModifyZeroData(data);
            foreach (var item in data)
                _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return data;
        }

        private void ModifyZeroData(List<CovidData> data)
        {
            for (int i = 1; i < data.Count; i++)
            {
                if (data[i].Confirm < data[i - 1].Confirm) data[i].Confirm = data[i - 1].Confirm;
                if (data[i].Recovered < data[i - 1].Recovered) data[i].Recovered = data[i - 1].Recovered;
                if (data[i].Deaths < data[i - 1].Deaths) data[i].Deaths = data[i - 1].Deaths;
            }            
        }

        private async Task<JArray> CallGetRequest(string country, string from, string to)
        {
            Console.WriteLine(await _context.Countries.FromSqlRaw($"SELECT * FROM Countries where CountryName = '{country}'").ToListAsync());
            List<CountryData> iso = await _context.Countries.FromSqlRaw($"SELECT * FROM Countries where CountryName = '{country}'").ToListAsync();
            var request = new GetRequest($"https://api.covid19api.com/country/{iso[0].ISO2}?from={from}&to={to}");
            var response = await request.Run();
            if (response != null)
            {
                var json = JArray.Parse(response);                
                return json;
            }
            return null;
        }
    }
}
