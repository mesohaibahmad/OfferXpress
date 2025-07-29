

using OffersForCustomers.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using OffersForCustomers.Model;
using DinkToPdf.Contracts;
using System.Text;
using System.Collections;
using Microsoft.Extensions.Hosting;
////using Microsoft.Data.SqlClient;
//using OffersForCustomers.DAL;

namespace OffersForCustomers.Services
{
    public interface IOffersListServices
    {
        // Correct method name
        public Task<List<Offers>> GetGeneratedOffers(int days, int countryId);
      

        //Task<List<Dictionary<string, object>>> DashboardData(int year, int month);
    }
    public class OffersListServices : IOffersListServices
    {
        private readonly OffersForCustomersDbContext dBContext;
        private readonly IWebHostEnvironment environment;
        public IConfiguration Configuration { get; set; }
        private readonly IConverter _converter;
        public OffersListServices(OffersForCustomersDbContext _dbContext, IWebHostEnvironment _environment
            , IConfiguration configuration, IConverter converter)
        {
            dBContext = _dbContext;
            environment = _environment;
            Configuration = configuration;
            _converter = converter;
        }

        public async Task<List<Offers>> GetGeneratedOffers(int days, int countryId)
        {
         
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            var list = await dBContext.Offers
             .Where(o => o.CreationDatetime >= cutoffDate && o.CountryId == countryId && o.PDFGenerated == true)
             .ToListAsync();
            if (days == -1)
            {
                list = await dBContext.Offers
             .Where(o => o.CountryId == countryId && o.PDFGenerated == true)
             .ToListAsync();
            }
            if(days != -1)
            {
                list = list.FindAll(l => l.CreationDatetime >= cutoffDate);
            }

                return list;
        }
      

    }
}