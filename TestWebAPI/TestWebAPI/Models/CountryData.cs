using Newtonsoft.Json;

namespace TestWebAPI.Models.CountriesData
{
    public class CountryData
    {
        public int Id { get; set; }
        [JsonProperty("Country")]
        public string CountryName { get; set; }
        [JsonProperty("Slug")]
        public string Slug { get; set; }
        [JsonProperty("ISO2")]
        public string ISO2 { get; set; }
    }
}
