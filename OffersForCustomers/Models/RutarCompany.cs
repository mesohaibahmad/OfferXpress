namespace OffersForCustomers.Models
{
    public class RutarCompany
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string VATNumber { get; set; }
        public string IBAN1 { get; set; }
        public string BIC1 { get; set; }
        public string IBAN2 { get; set; }
        public string BIC2 { get; set; }

        //// Navigation properties
        //public virtual ICollection<Branches> Branches { get; set; }
    }

}
