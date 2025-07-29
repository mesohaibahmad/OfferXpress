using System;

namespace OffersForCustomers.Models
{
    public class OfferedItemRows
    {
        public long Id { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public long OfferId { get; set; }
        public long? ProductNo { get; set; }
        public string ProductDescription { get; set; }
        public int QTY { get; set; }
        public decimal PricePerUnitInclVAT { get; set; }
        public decimal PricePerUnitExclVAT { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public bool IsEdited { get; set; }


    }

}
