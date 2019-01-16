using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsStore.Models
{
    public class ShowOrderView
    {
        public ShowOrderView(Order item, IEnumerable<Product> lst)
        {
            this._order = item;
            this.products = lst;
        }

        public Order _order { get; set; }
        public IEnumerable<Product> products { get; set; }

    }
}
