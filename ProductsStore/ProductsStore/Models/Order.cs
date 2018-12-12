using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class Order
    {
        public Order()
        {
            this.Products = new HashSet<ProductOrders>();
        }

        public int ID { get; set; }
        public int userID { get; set; }
        [Required]
        public Nullable<System.DateTime> orderDate { get; set; }
        [Display(Name = "credit Card Num")]
        [Required]
        public string creditCardNum { get; set; }
        public Nullable<int> amount { get; set; }

        public virtual ICollection<ProductOrders> Products { get; set; }
        public virtual User User { get; set; }
    }
}