namespace OfferXpress.Models
{
    public class Offers
    {
        public long Id { get; set; }
        public long OfferId { get; set; }
        public DateTime CreationDatetime { get; set; }
        public long BranchId { get; set; }
        public string CustomerCompanyName { get; set; }
        public char? CustomerGender { get; set; } // 'M' or 'F'
        public string CustomerFirstname { get; set; }
        public string CustomerLastname { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string ProjectName { get; set; }
        public string SalespersonName { get; set; }
        public string SalespersonPhoneNumber { get; set; }
        public string SalespersonEmail { get; set; }
        public DateTime ValidUntil { get; set; }
        public int DeliveryTimeInWeeks { get; set; }
        public decimal RequiredPrepayment { get; set; } = 50.00m; // Default value
        public string PdfPath { get; set; }
        public bool? PDFGenerated { get; set; }
        public long CountryId { get; set; }


    }
}
