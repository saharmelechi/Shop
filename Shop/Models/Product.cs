using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Product
    {

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public int Category { get; set; }

        public double Price { get; set; }

        public string Name{ get; set; }

    }
}
