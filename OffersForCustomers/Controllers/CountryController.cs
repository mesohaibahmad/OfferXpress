using Microsoft.AspNetCore.Mvc;
using OffersForCustomers.Models; // Assuming you have your models in this namespace

namespace OffersForCustomers.Controllers
{
    [Route("Countries")]
    public class CountryController : Controller
    {
        private readonly OffersForCustomersDbContext _context;

        public CountryController(OffersForCustomersDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var countries = _context.Countries.ToList(); // Fetch all countries

            if (!countries.Any())
            {
                ViewBag.Message = "No countries found in the database.";
            }

            return View(countries); // Pass the list of countries to the view
        }
    }
}
