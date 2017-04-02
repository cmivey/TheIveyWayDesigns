namespace TheIveyWayDesigns.Models
{
    public class PackingSlipModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string OrderTotal { get; set; }
        public int LineNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}
