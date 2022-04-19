using Microsoft.EntityFrameworkCore;
using TestWebAPI.Models.CountriesData;
using TestWebAPI.Models.CovidData;

namespace TestWebAPI.Database
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<CountryData> Countries { get; set; }
        public DbSet<CovidData> CovidDatas { get; set; }
    }
}
