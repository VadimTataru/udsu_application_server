using Newtonsoft.Json;
using System;

namespace TestWebAPI.Models.ThirdPartyApiModels
{
    public class CovidDataJson
    {
        [JsonProperty("Country")]
        public string CountryName { get; set; }
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("Confirmed")]
        public int Confirmed { get; set; }
        [JsonProperty("Recovered")]
        public int Recovered { get; set; }
        [JsonProperty("Deaths")]
        public int Deaths { get; set; }
    }
}
