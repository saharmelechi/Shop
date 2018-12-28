using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class Address
    {
        public int ID { get; set; }
        public string City{ get; set; }
        public string Street { get; set; }
        public string CountOfProducts { get; set; }
    }
}