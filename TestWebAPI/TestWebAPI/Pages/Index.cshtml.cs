using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestWebAPI.Models.CountriesData;
using TestWebAPI.Models.ThirdPartyApiModels;
using TestWebAPI.ThirdPartyCoivdAPI;

namespace TestWebAPI.Pages
{
    public class IndexModel : PageModel
    {
        public record class RequestData(string Country, DateTime date_from, DateTime date_to);
        #region var args
        [BindProperty]
        public RequestData requestData { get; set; } = new RequestData("", DateTime.MinValue, DateTime.MaxValue);
        public List<CountryDataJson>? Countries { get; private set; }
        public List<CovidDataJson>? CovidDatas { get; private set;}
        public string Country { get; private set; } = "";
        public List<string> DateLabels { get; private set; } = new List<string>();
        public List<int> Confirmed { get; private set; } = new List<int>();
        public List<int> Recovered { get; private set; } = new List<int>();
        public List<int> Deaths { get; private set; } = new List<int>();
        public DateTime maxDate_to { get; } = DateTime.Now;
        public DateTime maxDate_from { get; set; } = DateTime.Now.AddDays(-2);
        public DateTime minDate { get; } = new DateTime(2020, 1, 22);
        public string Mess { get; set; } = "";
        #endregion
        public async Task OnGet()
        {
            Countries = await GetCountryData();
        }

        public async Task OnPost()
        {
            CovidDatas = await GetCovidData(requestData.Country, requestData.date_from, requestData.date_to);
            await OnGet();
        }        

        public async Task<List<CountryDataJson>> GetCountryData()
        {
            var request = new GetRequest("https://localhost:5001/api/country");            
            var response = await request.Run();

            if (response != null)
            {
                var json = JArray.Parse(response);
                List<CountryDataJson>?  countries = JsonConvert.DeserializeObject<List<CountryDataJson>>(json.ToString());
                return countries;
            }
            return null;
        }

        public async Task<List<CovidDataJson>> GetCovidData(string country, DateTime from, DateTime to)
        {
            string date_from = from.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string date_to = to.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var request = new GetRequest($"https://localhost:5001/api/CovidData?country={country}&date_from={date_from}&date_to={date_to}");

            var response = await request.Run();

            if (response != null)
            {
                var json = JArray.Parse(response);
                List<CovidDataJson>? covidDatas = JsonConvert.DeserializeObject<List<CovidDataJson>>(json.ToString());

                foreach (var cd in covidDatas)
                {
                    Confirmed.Add(cd.Confirmed);
                    Recovered.Add(cd.Recovered);
                    Deaths.Add(cd.Deaths);
                    DateLabels.Add(cd.Date.ToString("yyyy-MM-dd"));
                }
                Country = country;
                return covidDatas;
            }
            return null;
        }
    }
}
