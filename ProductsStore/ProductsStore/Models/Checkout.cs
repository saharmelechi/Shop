using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class Checkout
    {
        public  string CreditCard{ get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "UPRN must be numeric")]
        public int Month { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "UPRN must be numeric")]
        public int Year { get; set; }
        public int? TotalAmount { get; set; }

    }
}