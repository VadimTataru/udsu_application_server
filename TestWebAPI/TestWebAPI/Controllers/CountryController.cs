using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Models;
using TestWebAPI.Models.CountriesData;
using TestWebAPI.Repositories.CountryRepository;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CountryData>> Countries()
        {
            return await _countryRepository.Get();
        }
    }
}
