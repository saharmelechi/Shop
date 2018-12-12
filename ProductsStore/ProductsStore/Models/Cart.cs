using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; }
        public int? TotalAmount { get; set; }

        public Cart()
        {
            this.Products = new List<Product>();
            this.TotalAmount = 0;
        }
    }
}