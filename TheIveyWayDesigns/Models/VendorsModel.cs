namespace TheIveyWayDesigns.Models
{
    public class VendorsModel
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
    }

    public class VendorProductsModel
    {
        public int VendorProductsId { get; set; }
        public int VendorId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
