// Location: YourProject/Components/CountriesViewComponent.cs

using Microsoft.AspNetCore.Mvc;
using OfferXpress.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CountriesViewComponent : ViewComponent
{
    private readonly ICountryListService _countryService;

    public CountriesViewComponent(ICountryListService countryService)
    {
        _countryService = countryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var countries = await _countryService.GetCountries();
        return View(countries); // Return the view with the countries data
    }
}
