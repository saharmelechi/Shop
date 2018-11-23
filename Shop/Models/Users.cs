using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Users
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public Levels AuthLevel { get; set; }

        public DateTime BirthDay { get; set; }

        public string Address { get; set; }

    }

    public enum Levels : int
    {
        Manager = 0,
        Employee = 2,
        Customer = 4,
        Guest = 8
    };
}
