using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebDelivery2.Models;
using static WebDelivery2.Models.@enum;

namespace WebDelivery2;

public partial class DeliveryContext : DbContext
{
    public DeliveryContext()
    {
    }

    public DeliveryContext(DbContextOptions<DeliveryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=delivery;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<OrderStatus>("order_status");

        modelBuilder.HasPostgresEnum("order_status", ["pending", "intransit", "delivered"]);

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_pkey");

            entity.ToTable("addresses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.House)
                .HasMaxLength(20)
                .HasColumnName("house");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("addresses_customer_id_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Email, "customers_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Money)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("money");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("drivers_pkey");

            entity.ToTable("drivers");

            entity.HasIndex(e => e.Email, "drivers_email_key").IsUnique();

            entity.HasIndex(e => e.Licensenumber, "drivers_licensenumber_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Licensenumber)
                .HasMaxLength(20)
                .HasColumnName("licensenumber");
            entity.Property(e => e.Money)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("money");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Rating)
                .HasPrecision(3, 2)
                .HasColumnName("rating");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.DeliveryaddressId).HasColumnName("deliveryaddress_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.PickupaddressId).HasColumnName("pickupaddress_id");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("orders_customer_id_fkey");

            entity.HasOne(d => d.Deliveryaddress).WithMany(p => p.OrderDeliveryaddresses)
                .HasForeignKey(d => d.DeliveryaddressId)
                .HasConstraintName("orders_deliveryaddress_id_fkey");

            entity.HasOne(d => d.Driver).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("orders_driver_id_fkey");

            entity.HasOne(d => d.Pickupaddress).WithMany(p => p.OrderPickupaddresses)
                .HasForeignKey(d => d.PickupaddressId)
                .HasConstraintName("orders_pickupaddress_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
