
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using OffersForCustomers.Models;
using OffersForCustomers.Model;
using OffersForCustomers.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace OffersForCustomers.Controllers
{
    public class OffersListController : Controller
    {
        private readonly ILogger<OffersListController> _logger;
        private readonly IOffersListServices offersListServices;
        private readonly IStringLocalizer<HomeController> _localizer;

        public OffersListController(ILogger<OffersListController> logger, IOffersListServices _oFfersListServices, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            offersListServices = _oFfersListServices;
            _localizer = localizer;
        }


        [HttpGet]
        [Route("OffersList/Generated")]
        public async Task<IActionResult> Index(int days = 3)
        {
           
                var CountryId = Convert.ToInt32(HttpContext.Session.GetString("SelectedCountryId"));
                var list = await offersListServices.GetGeneratedOffers(days, CountryId);

                return View(list);
      
    
            //var model = await commonServices.GetObjects();
            //  var obj=await commonServices.TotalUserCounts();
            //  ViewData["TotalUsers"] = obj;
           
        }

        [HttpPost]
        public IActionResult SelectCountry(int countryId)
        {
            if (countryId > 0)
            {
                HttpContext.Session.SetString("SelectedCountryId", countryId.ToString());


            }
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            return RedirectToAction("Index", controllerName); // Redirect back to the index or another action
        }



        public async Task<IActionResult> GetOffers(int days)
        {
            try
            {
                var CountryId = Convert.ToInt32(HttpContext.Session.GetString("SelectedCountryId"));
                var list = await offersListServices.GetGeneratedOffers(days, CountryId);

                return Ok(list);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
