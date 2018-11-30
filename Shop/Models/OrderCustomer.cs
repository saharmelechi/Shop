using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class OrderCustomer
    {
        public int IDCustomer { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public DateTime OraderTime { get; set; }

        public double Price { get; set; }

    }
}
