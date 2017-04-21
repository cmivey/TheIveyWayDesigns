using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIveyWayDesigns.Models
{
    public class InventoryModel
    {
        public int InventoryId { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public double Quantity { get; set; }
    }
}
