
using OfferXpress.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using OfferXpress.Model;
using DinkToPdf.Contracts;
using System.Text;
using System.Collections;
////using Microsoft.Data.SqlClient;
//using OfferXpress.DAL;

namespace OfferXpress.Services
{
    public interface ICommonServices
    {
        public  Task<InputLists> GetObjects();
        public Task<string> GeneratePdfAndSave(FormModel offer, string selectedLanguage);
        public Task<FormModel> EditOffer(int id);

        //Task<List<Dictionary<string, object>>> DashboardData(int year, int month);
    }
    public class CommonServices : ICommonServices
    {
        private readonly OfferXpressDbContext dBContext;
        private readonly IWebHostEnvironment environment;
        public IConfiguration Configuration { get; set; }
        private readonly IConverter _converter;
        public CommonServices(OfferXpressDbContext _dbContext, IWebHostEnvironment _environment
            , IConfiguration configuration, IConverter converter)
        {
            dBContext = _dbContext;
            environment = _environment;
            Configuration = configuration;
            _converter = converter;
        }

        public async Task<InputLists> GetObjects()
        {
            InputLists obj = new InputLists();
            obj.BranchList = await dBContext.Branches.ToListAsync();
            obj.CountryList = await dBContext.Countries.ToListAsync();
             
        

            return obj;

        }
        
        public async Task<string> GeneratePdfAndSave(FormModel offer, string selectedLanguage)
        {
            try
            {
                long newOfferId = 0;
                long offerId = 1;

                var findOffer = await dBContext.Offers.AsNoTracking().Where(x => x.OfferId == offer.OfferId && x.PDFGenerated == true).FirstOrDefaultAsync();
                var findOfferedItems = await dBContext.OfferedItemRows.AsNoTracking().Where(x => x.OfferId == offer.OfferId).ToListAsync();

                if (findOffer != null)
                {
                    findOffer.PDFGenerated = false;
                    dBContext.Offers.Update(findOffer);

                    offerId = offer.OfferId;


                }
                else
                {
                    var objcat = dBContext.Offers.OrderByDescending(x => x.OfferId).FirstOrDefault();

                    if (objcat != null)
                    {
                        offerId = objcat.OfferId + 1;
                    }
                  
                }

                if (findOfferedItems != null)
                {
                    foreach (var row in findOfferedItems)
                    {
                        row.IsEdited = true;
                        dBContext.OfferedItemRows.Update(row);
                    }

                }

               


                var param = new Offers()
                {
                    OfferId = offerId,
                    CreationDatetime =   DateTime.UtcNow,
                    BranchId = offer.BranchId,
                    CustomerCompanyName = offer.CustomerCompanyName,
                    CustomerGender = 'N',
                    CustomerFirstname = offer.CustomerFirstname,
                    CustomerLastname = offer.CustomerLastname,
                    CustomerAddress = offer.CustomerAddress,
                    CustomerCity = offer.CustomerCity,
                    ProjectName = offer.ProjectName,
                    SalespersonName = offer.SalespersonName,
                    SalespersonPhoneNumber = offer.SalespersonPhoneNumber,
                    SalespersonEmail = offer.SalespersonEmail,
                    ValidUntil = offer.ValidUntil ,
                    DeliveryTimeInWeeks = offer.DeliveryTimeInWeeks,
                    RequiredPrepayment = offer.RequiredPrepayment,
                    CountryId = offer.CountryId,
                    PDFGenerated = false,
                    PdfPath = "",


                };
                var offer1 = dBContext.Offers.Add(param);
                await dBContext.SaveChangesAsync();

                newOfferId = offer1.Entity.OfferId;
                offer.CreationDatetime = offer1.Entity.CreationDatetime;


                foreach (var item in offer.OfferedItems)
                {

                    var param2 = new OfferedItemRows()
                    {

                        CreationDatetime = DateTime.UtcNow,

                        ProductNo = item.ProductNo,
                        ProductDescription = item.ProductDescription,
                        QTY = item.QTY,
                        PricePerUnitInclVAT = Math.Round(item.PricePerUnitInclVAT, 2),
                        PricePerUnitExclVAT = item.PricePerUnitExclVAT,
                        VATPercentage = item.VATPercentage,
                        VATAmount = item.VATAmount,
                        OfferId = newOfferId,
                        IsEdited = false
                    };


                    dBContext.OfferedItemRows.Add(param2);
                    await dBContext.SaveChangesAsync();

                };

                    
                

                if (newOfferId > 0)
                {
                    var htmlContent = await GenerateHtml(offer, selectedLanguage);
                    var pdf = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10,
                Bottom = 10,
                Left = 10,
                Right = 10 }
                },
                        Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" },
                    }
                        }
                    };
                    if (pdf != null)
                    {
                        offer1.Entity.PDFGenerated = true;
                        await dBContext.SaveChangesAsync();

                    }
                 
                    byte[] pdfBytes = _converter.Convert(pdf);

                    // Define the path to save the PDF
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","PDFs"); // Change "StoredPDFs" to your desired folder name
                    string fileName = $"Offer_{offer.CustomerFirstname}_{offer.CustomerLastname}_{DateTime.Now:ddMMyyyy_HHmmss}.pdf";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Save the PDF to the defined path
                    await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

                    offer1.Entity.PdfPath = $"/PDFs/{fileName}";
                    await dBContext.SaveChangesAsync();

                    return fileName;
                }

                return "Error";

            

            }






                //            var items = new List<OfferedItemRow>
                //{
                //    new OfferedItemRow { ProductDescription = "Product 1", QTY = 2, Price_per_unit_incl_VAT = 120.00m, VAT_Percentage = 22 },
                //    new OfferedItemRow { ProductDescription = "Product 2", QTY = 1, Price_per_unit_incl_VAT = 100.00m, VAT_Percentage = 22 }
                //};


                //Calculate totals
                //    decimal totalAmountInclVAT = 0;
                //decimal totalAmountExclVAT = 0;

                //foreach (var item in offer.OfferedItemRows)
                //{
                //    totalAmountInclVAT += item.Price_per_unit_incl_VAT * item.QTY;
                //    totalAmountExclVAT += item.Price_per_unit_excl_VAT * item.QTY; // Assuming you calculate this
                //}
   




            catch(Exception ex)
            {
                return (ex.Message);
            }


        }
        public async Task<string> GetBranchName(long id)
        {
            var obj = await GetObjects();
            var branch = obj.BranchList.Find(b => b.Id == id); // Assuming 'Id' is the property name

            return branch?.BranchName; // Returns the branch name or null if not found
        }
        private async Task<string> GenerateHtml(FormModel offer, string selectedLanguage)
{
            decimal totalAmountInclVAT = 0;
            decimal totalAmountExclVAT = 0;
            decimal totalVAT = 0;

            foreach (var item in offer.OfferedItems)
            {
                totalAmountInclVAT += item.PricePerUnitInclVAT * item.QTY;
                totalAmountExclVAT += item.PricePerUnitExclVAT * item.QTY; // Assuming you calculate this
                totalVAT += item.VATPercentage;
            }

           

            string branchName = await GetBranchName(offer.BranchId);

            var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", selectedLanguage switch
            {
                "de" => "OfferTemplate_de.html",
                "it" => "OfferTemplate_it.html",
                "sl" => "OfferTemplate_sl.html",
                _ => "OfferTemplate_en.html", // default to English
            });
    

            var htmlTemplate = File.ReadAllText(templateFilePath);

            // Replace placeholders with actual values
            return htmlTemplate
        .Replace("{{Customer_Company_Name}}", offer.CustomerCompanyName)
        .Replace("{{Customer_Firstname}}", offer.CustomerFirstname)
        .Replace("{{Customer_Lastname}}", offer.CustomerLastname)
        .Replace("{{Customer_Address}}", offer.CustomerAddress)
        .Replace("{{Customer_City}}", offer.CustomerCity)
        .Replace("{{Branch_Name}}", branchName)
        .Replace("{{Creation_datetime}}", offer.CreationDatetime.ToString("dd.MM.yyyy"))
        .Replace("{{Salesperson_Name}}", offer.SalespersonName)
        .Replace("{{Salesperson_phone_number}}", offer.SalespersonPhoneNumber)
        .Replace("{{Salesperson_email}}", offer.SalespersonEmail)
        .Replace("{{Project_Name}}", offer.ProjectName)
        .Replace("{{Valid_Until}}", offer.ValidUntil.ToString("dd.MM.yyyy"))
        .Replace("{{Required_Prepayment}}", offer.RequiredPrepayment.ToString())
        .Replace("{{Delivery_Time_in_Weeks}}", offer.DeliveryTimeInWeeks.ToString())
        .Replace("{{TotalExclVAT}}", totalAmountExclVAT.ToString("F2"))
        .Replace("{{TotalVAT}}", totalVAT.ToString("F2"))
        .Replace("{{TotalAmount}}", totalAmountInclVAT.ToString("F2"))
        .Replace("{{Items}}", GenerateItemsHtml(offer.OfferedItems));
}
      

private string GenerateItemsHtml(ICollection<OfferedItemRows> items)
{

            decimal AmountInclVAT = 0;
            decimal AmountExclVAT = 0;

        
            var sb = new StringBuilder();
    foreach (var item in items)
    {
                item.PricePerUnitExclVAT = Math.Round(item.PricePerUnitInclVAT, 2);
                AmountInclVAT = item.PricePerUnitInclVAT * item.QTY;
                AmountExclVAT = item.PricePerUnitExclVAT * item.QTY;

                sb.Append("<tr>")
          .Append($"<td>{item.ProductDescription}</td>")
          .Append($"<td>{item.QTY}</td>")
          .Append($"<td>{item.PricePerUnitInclVAT} €</td>")
          .Append($"<td>{item.VATPercentage} %</td>")
          .Append($"<td>{AmountInclVAT} €</td>")
          .Append($"<td>{AmountExclVAT} €</td>")
          .Append("</tr>");
    }
    return sb.ToString();
}
        public async Task<FormModel> EditOffer(int id)
        {
            var offer = dBContext.Offers.FirstOrDefault(o => o.OfferId == id && o.PDFGenerated == true);

            var itemRows = dBContext.OfferedItemRows
           .Where(r => r.OfferId == id && r.IsEdited == false ) // Assuming UserId is used to relate posts to users
           .ToList();
            var formModel = new FormModel
            {
                OfferId = offer.OfferId,
                CreationDatetime = offer.CreationDatetime,
                BranchId = offer.BranchId,
                CustomerCompanyName = offer.CustomerCompanyName,
                CustomerGender = offer.CustomerGender,
                CustomerFirstname = offer.CustomerFirstname,
                CustomerLastname = offer.CustomerLastname,
                CustomerAddress = offer.CustomerAddress,
                CustomerCity = offer.CustomerCity,
                ProjectName = offer.ProjectName,
                SalespersonName = offer.SalespersonName,
                SalespersonPhoneNumber = offer.SalespersonPhoneNumber,
                SalespersonEmail = offer.SalespersonEmail,
                ValidUntil = offer.ValidUntil,
                DeliveryTimeInWeeks = offer.DeliveryTimeInWeeks,
                RequiredPrepayment = offer.RequiredPrepayment,
                PdfPath = offer.PdfPath,
                CountryId = offer.CountryId,
                PDFGenerated = offer.PDFGenerated,
                OfferedItems = itemRows
            };


            return formModel;
        }
    }
   
}
