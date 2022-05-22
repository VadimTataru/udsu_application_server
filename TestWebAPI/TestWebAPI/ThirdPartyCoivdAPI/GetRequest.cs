using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestWebAPI.Models.CountriesData;
using TestWebAPI.Models.CovidData;

namespace TestWebAPI.ThirdPartyCoivdAPI
{
    public class GetRequest
    {
        string addressUrl;

        public string? responseData { get; set; }

        public GetRequest(string addressUrl)
        {
            this.addressUrl = addressUrl;
        }

        public async Task<string> Run()
        {
            var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            try
            {
                var response = await client.GetAsync(addressUrl);
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                
                var responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
