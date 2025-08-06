using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using OfferXpress.Model;
using OfferXpress.Models;

namespace OfferXpress.Services
{
    public interface ICountryListService
    {
        public Task<List<Countries>> GetCountries();

        //Task<List<Dictionary<string, object>>> DashboardData(int year, int month);
    }
    public class CountryListService : ICountryListService
    {
        private readonly OfferXpressDbContext dBContext;
        private readonly IWebHostEnvironment environment;
        public IConfiguration Configuration { get; set; }
 
        public CountryListService(OfferXpressDbContext _dbContext, IWebHostEnvironment _environment
            , IConfiguration configuration)
        {
            dBContext = _dbContext;
            environment = _environment;
            Configuration = configuration;
         
        }
        public async Task<List<Countries>> GetCountries() // Fixed method name
        {
            var countries = await dBContext.Countries.ToListAsync();
            return countries;
        }
    }
}
