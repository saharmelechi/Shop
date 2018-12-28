﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductsStore.Models;

namespace ProductsStore.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20181226153639_create9")]
    partial class create9
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductsStore.Models.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("CountOfProducts");

                    b.Property<string>("Street");

                    b.HasKey("ID");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("ProductsStore.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("creditCardNum")
                        .IsRequired();

                    b.Property<DateTime?>("orderDate")
                        .IsRequired();

                    b.Property<int>("userID");

                    b.HasKey("ID");

                    b.HasIndex("userID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("ProductsStore.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("image")
                        .IsRequired();

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int?>("price")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("ProductsStore.Models.ProductOrders", b =>
                {
                    b.Property<int>("poID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountOfProducts");

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.HasKey("poID");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOrders");
                });

            modelBuilder.Entity("ProductsStore.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("birthDate");

                    b.Property<string>("email")
                        .IsRequired();

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<bool>("isAdmin");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("pass")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ProductsStore.Models.Order", b =>
                {
                    b.HasOne("ProductsStore.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProductsStore.Models.ProductOrders", b =>
                {
                    b.HasOne("ProductsStore.Models.Order", "_order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProductsStore.Models.Product", "_product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
