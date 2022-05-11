using Microsoft.EntityFrameworkCore;
using TestWebAPI.Database;
using TestWebAPI.Repositories.CountryRepository;
using TestWebAPI.Repositories.CovidDataRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ICovidDataRepository, CovidDataRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddDbContext<ApplicationContext>(o => o.UseSqlite("Data source=appdata.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidDataAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();


//api.covid19api.com/countries to get country names
//api.covid19api.com/country/{country_name} to get data by country name
//api.covid19api.com/country/{country_name}?from=YYYY-mm-ddT00:00:00Z&to=YYYY-mm-ddT00:00:00Z to get data by country name
