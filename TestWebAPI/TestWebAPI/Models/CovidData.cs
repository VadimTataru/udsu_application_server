using System;

namespace TestWebAPI.Models.CovidData
{
    public class CovidData
    {
        public string CountryName { get; set; }
        public DateTime Date { get; set; }
        public int Confirm { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
    }
}
