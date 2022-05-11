using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models.CovidData
{
    public class CovidData
    {
        [Required]
        [JsonProperty("Country")]
        public string CountryName { get; set; }
        [Required]
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("Confirmed")]
        public int Confirm { get; set; }
        [JsonProperty("Recovered")]
        public int Recovered { get; set; }
        [JsonProperty("Deaths")]
        public int Deaths { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is CovidData covidData) return Date == covidData.Date;
            return false;
        }
        public override int GetHashCode() => Date.GetHashCode();
    }
}
