using OfferXpress.Models;
using Microsoft.EntityFrameworkCore;
using OfferXpress.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICommonServices, CommonServices>();
builder.Services.AddScoped<IOffersListServices, OffersListServices>();
builder.Services.AddScoped<ICountryListService, CountryListService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Preserve property names (case-sensitive)
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });


try
{
    builder.Services.AddLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}


var supportedCultures = new[] { "en-US", "de-GR", "it-IT", "sl-Sl" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US"); // Set the default culture
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = options.SupportedCultures;
});


builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");


//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    var supportedCultures = new CultureInfo[]
//    {
//        new CultureInfo("en-US"),
//        new CultureInfo("de-GR"),
//        new CultureInfo("it-IT"),
//        new CultureInfo("sl-Sl")
//    };

//    options.DefaultRequestCulture = new RequestCulture("en-US");

//    options.FallBackToParentCultures = true; // Enables fallback to parent cultures
//    options.FallBackToParentUICultures = true;

//    options.SupportedCultures = supportedCultures;
//    options.SupportedUICultures = supportedCultures;

//    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
//});

//try
//{

//    builder.Services.AddControllersWithViews()
//        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
//    .AddDataAnnotationsLocalization();

//}
//catch (Exception ex)
//{

//    Console.WriteLine(ex);
//}


builder.Services.AddDbContext<OfferXpressDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr"));
});

var app = builder.Build();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();