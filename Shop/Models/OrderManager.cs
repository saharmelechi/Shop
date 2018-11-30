using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class OrderManager
    {
        public int IDManager { get; set; }

        public DateTime OrderTime { get; set; }

        public IEnumerable<Product> Products { get; set; }

                
    }
}
