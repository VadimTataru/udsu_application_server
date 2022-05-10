using Microsoft.EntityFrameworkCore;
using TestWebAPI.Models.CountriesData;
using TestWebAPI.Models.CovidData;

namespace TestWebAPI.Database
{
    public class ApplicationContext: DbContext
    {
        public DbSet<CountryData> Countries { get; set; } = null!;
        public DbSet<CovidData> CovidDatas { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryData>().HasKey(c => new { c.Id });
            modelBuilder.Entity<CovidData>().HasKey(cov => new { cov.Date, cov.CountryName });
        }
    }
}
