using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ProductsStore.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; }
        public int? TotalAmount { get; set; }

        public Cart()
        {
            this.Products = new List<Product>();
            this.TotalAmount = 0;
        }

        public static explicit operator Cart(string s)
        {
            return JsonConvert.DeserializeObject<Cart>(s);
        }
        public static implicit operator string(Cart cart)
        {
            return JsonConvert.SerializeObject(cart);
        }

    }
}