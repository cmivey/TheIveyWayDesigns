using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIveyWayDesigns
{
    public class OrdersModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Shipped { get; set; }

        public double OrderTotal { get; set; }
    }

    public class OrderDetailsModel
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}


