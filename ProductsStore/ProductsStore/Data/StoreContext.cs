using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsStore.Models;

namespace ProductsStore.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public DbSet<ProductsStore.Models.Product> Product { get; set; }

        public DbSet<ProductsStore.Models.Order> Order { get; set; }

        public DbSet<ProductsStore.Models.Address> Address { get; set; }

        public DbSet<ProductsStore.Models.User> User { get; set; }

        public DbSet<ProductsStore.Models.ProductOrders> ProductOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Order>()
                .HasKey(t => t.ID);

            modelBuilder.Entity<Product>()
                .HasKey(t => t.ID);

            modelBuilder.Entity<ProductOrders>().HasKey(x => x.poID);

            modelBuilder.Entity<ProductOrders>()
                .HasOne(x => x._product)
                .WithMany(m => m.Orders)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<ProductOrders>()
                .HasOne(x => x._order)
                .WithMany(e => e.Products)
                .HasForeignKey(x => x.OrderId);
                
            modelBuilder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(u => u.userID);

            base.OnModelCreating(modelBuilder);

        }








    }



}
