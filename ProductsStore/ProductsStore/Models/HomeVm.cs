using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class HomeVm
    {
        public List<Product> Products { get; set; }
        public Product TopSale { get; set; }
    }
}