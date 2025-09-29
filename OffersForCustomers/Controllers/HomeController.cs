using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using OfferXpress.Models;
using OfferXpress.Model;
using OfferXpress.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace OfferXpress.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonServices commonServices;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, ICommonServices _commonServices,
            IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            commonServices = _commonServices;
            _localizer = localizer;
        }


        public async Task<IActionResult> Index()
        {
            var model = await commonServices.GetObjects();

            ViewBag.Countries = model.CountryList;
            //  var obj=await commonServices.TotalUserCounts();
            //  ViewData["TotalUsers"] = obj;

            return View(model);
        }


        [HttpPost]
        [Route("home/SavePdf")]
        public async Task<IActionResult> SaveAndGenerate([FromBody] FormModel offer)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentUICulture;
                var selectedLanguage = currentCulture.TwoLetterISOLanguageName;
                // Use async method to avoid blocking
                var fileName = await commonServices.GeneratePdfAndSave(offer, selectedLanguage);

                return Ok(new { pdfUrl = Url.Content($"/pdfs/{fileName}") });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("home/editOffer")]
        public async Task<IActionResult> GetOfferDetails(int id)
        {

            var offerData = await commonServices.EditOffer(id);

            if (offerData == null)
            {
                return NotFound();
            }

            return Ok(offerData);
        }

        [HttpPost]
        [Route("home/SetLanguage")]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}


