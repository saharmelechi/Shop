using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class Product
    {
        public Product()
        {

            this.Orders = new HashSet<ProductOrders>();

        }

        public int ID { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        [StringLength(20)]
        public string name { get; set; }
        [Required]
        public Nullable<int> price { get; set; }
        [Display(Name = "Description")]
        [Required]
        [StringLength(20)]
        public string description { get; set; }
        [Required]
        public string image { get; set; }

        public virtual ICollection<ProductOrders> Orders { get; set; }
    }
}