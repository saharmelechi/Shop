using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsStore.Models
{
    public class ProductOrders
    {
        public int ProductId { get; set; }

        public Product _product { get; set; }

        public int OrderId { get; set; }

        public Order _order { get; set; }


    }
}
