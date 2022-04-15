using Microsoft.EntityFrameworkCore;

namespace TestWebAPI.Models.CountriesData
{
    public class CountryDataContext: DbContext
    {
        public CountryDataContext(DbContextOptions<CountryDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<CountryData> Countries { get; set; }
    }
}
