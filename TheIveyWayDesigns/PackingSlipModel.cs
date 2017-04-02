using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIveyWayDesigns
{
    public class PackingSlipModel
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public int LineNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}
