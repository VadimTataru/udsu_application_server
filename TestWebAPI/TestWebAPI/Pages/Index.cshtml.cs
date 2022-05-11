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

        [BindProperty]
        public RequestData requestData { get; set; } = new RequestData("", DateTime.MinValue, DateTime.MaxValue);
        public List<CountryDataJson>? Countries { get; private set; }
        public async Task OnGet()
        {
            Countries = await GetCountryData();
        }

        public void OnPost()
        {
            //GetCovidData(requestData.Country, requestData.date_from, requestData.date_to);
        }        

        public async Task<List<CountryDataJson>> GetCountryData()
        {
            var request = new GetRequest("https://localhost:5001/api/country");            
            var response = await request.Run();

            if (response != null)
            {
                var json = JArray.Parse(response);
                Countries = JsonConvert.DeserializeObject<List<CountryDataJson>>(json.ToString());
                return Countries;
            }
            return null;
        }

        //Example request url https://localhost:5001/api/CovidData?country=russia&date_from=2020-01-07&date_to=2020-03-29
        //public async Task<List<CountryData>> GetCovidData(string country, DateTime from, DateTime to)
        //{
        //    string date_from = from.ToString("yyyy-MM-ddTHH:mm:ssZ");
        //    string date_to = to.ToString("yyyy-MM-ddTHH:mm:ssZ");
        //    var request = new GetRequest($"https://localhost:5001/api/CovidData?country={country}&date_from={date_from}&date_to={date_to}");
            
        //    var response = await request.Run();

        //    if (response != null)
        //    {
        //        var json = JArray.Parse(response);
        //        Countries = JsonConvert.DeserializeObject<List<CountryData>>(json.ToString());
        //        return Countries;
        //    }
        //    return null;
        //}
    }
}
