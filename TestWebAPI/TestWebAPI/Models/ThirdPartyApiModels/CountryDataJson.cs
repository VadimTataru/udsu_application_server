﻿using Newtonsoft.Json;

namespace TestWebAPI.Models.ThirdPartyApiModels
{
    public class CountryDataJson
    {
        [JsonProperty("Country")]
        public string Name { get; set; }
        [JsonProperty("Slug")]
        public string Slug { get; set; }
        [JsonProperty("ISO2")]
        public string CountryCode { get; set; }
    }
}