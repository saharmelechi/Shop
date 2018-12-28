using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductsStore.Models
{
    public class User
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
        }

        public int ID { get; set; }
        [Display(Name = "first Name")]
        [Required]
        [StringLength(20)]
        public string firstName { get; set; }
        [Display(Name = "last Name")]
        [Required]
        [StringLength(20)]
        public string lastName { get; set; }
        [Required(ErrorMessage ="password is requird")]
        [StringLength(20,ErrorMessage ="maximum 20 minimum 5" ,MinimumLength =5)]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string pass { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="error")]
        public string email { get; set; }
        [Display(Name = "birth Date")]
        public Nullable<System.DateTime> birthDate { get; set; }
        [Required]
        public bool isAdmin { get; set; }
       

        public virtual ICollection<Order> Orders { get; set; }

        public static explicit operator User(string s)
        {
            return JsonConvert.DeserializeObject<User>(s);
        }
        public static implicit operator string(User user)
        {
            return JsonConvert.SerializeObject(user);
        }
    }
}

