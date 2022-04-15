using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TestWebAPI.Models.CountriesData;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteCountryController : ControllerBase
    {
        string url = "https://jsonplaceholder.typicode.com/todos/1";
        
        /*public Task<IEnumerator<CountryData>> GetCountries()
        {
            string json = new WebClient().DownloadString(url);
            List<CountryData> items = JsonConvert.DeserializeObject<List<CountryData>>(json);
        }*/
    }
}
