using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Models
{
    public class MyShopContext : DbContext
    {
        public MyShopContext (DbContextOptions<MyShopContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Users>().HasKey(t => new { t.UserID });

        }

        public DbSet<Shop.Models.Category> Category { get; set; }

        public DbSet<Shop.Models.Product> Product { get; set; }

        public DbSet<Shop.Models.Users> Users { get; set; }
  
    }


}


