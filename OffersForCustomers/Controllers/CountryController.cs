using Microsoft.AspNetCore.Mvc;
using OfferXpress.Models; // Assuming you have your models in this namespace

namespace OfferXpress.Controllers
{
    [Route("Countries")]
    public class CountryController : Controller
    {
        private readonly OfferXpressDbContext _context;

        public CountryController(OfferXpressDbContext context)
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
