using System.Diagnostics.Metrics;

namespace OffersForCustomers.Models
{
    public class Branches
    {
        public long Id { get; set; }
        public string BranchName { get; set; }
        public string GroupName { get; set; }
        public long CountryId { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public long RutarCompanyId { get; set; }

        //// Navigation properties
        //public virtual Countries Country { get; set; }
        //public virtual RutarCompany RutarCompany { get; set; }
    }

}
