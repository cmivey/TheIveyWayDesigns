namespace TheIveyWayDesigns.Models
{
    public class AddOrderModel
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}
