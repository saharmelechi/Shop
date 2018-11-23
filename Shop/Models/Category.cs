using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }

    }
}
