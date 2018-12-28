using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsStore.Models
{
    public class ProductOrders
    {

        
        public int poID { get; set; }

        public int ProductId { get; set; }

        public Product _product { get; set; }

        public int OrderId { get; set; }

        public Order _order { get; set; }

        [Display(Name ="Count")]
        public int CountOfProducts { get; set; }

        public ProductOrders(int productId, int orderId)
            : this(productId, orderId, 0)
        {

        }
        public ProductOrders(int productId, int orderId, int count)
        {
            this.ProductId = productId;
            this.OrderId = orderId;
            this.CountOfProducts = count;
        }
    }
}
