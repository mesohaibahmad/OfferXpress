namespace OffersForCustomers.Models
{
    public class Countries
    {
        public long Id { get; set; }
        public string CountryName { get; set; }
        public decimal DefaultVAT { get; set; }

        //// Navigation properties
        //public virtual ICollection<Branches> Branches { get; set; }
    }

}
