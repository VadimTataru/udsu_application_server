using Newtonsoft.Json;

namespace TestWebAPI.Models.ThirdPartyApiModels
{
    public class CovidDataJson
    {
        [JsonProperty("CountryName")]
        public string CountryName { get; set; }
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("Confirm")]
        public int Confirmed { get; set; }
        [JsonProperty("Recovered")]
        public int Recovered { get; set; }
        [JsonProperty("Deaths")]
        public int Deaths { get; set; }

        public CovidDataJson(string countryName, DateTime date, int confirmed, int recoverd, int deaths)
        {
            CountryName = countryName;
            Date = date;
            Confirmed = confirmed;
            Recovered = recoverd;
            Deaths = deaths;
        }
    }
}
