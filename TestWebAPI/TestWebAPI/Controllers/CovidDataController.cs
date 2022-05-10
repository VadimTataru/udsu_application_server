using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Models.CovidData;
using TestWebAPI.Repositories.CovidDataRepository;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidDataController : Controller
    {
        private readonly ICovidDataRepository _covidRepository;

        public CovidDataController(ICovidDataRepository covidRepository)
        {
            _covidRepository = covidRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CovidData>> CoivdByDate(string country, DateTime date_from, DateTime date_to)
        {
            return await _covidRepository.GetWithDate(country, date_from, date_to);
        }

    }
}
