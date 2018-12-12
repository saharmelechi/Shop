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
        public Nullable<bool> single { get; set; }
        [Display(Name = "num of children")]
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "UPRN must be numeric")]
        public int numchildren { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

