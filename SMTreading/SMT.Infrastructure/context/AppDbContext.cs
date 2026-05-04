using Microsoft.EntityFrameworkCore;
using SMT.Domain.Common;
using SMT.Domain.Entities;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Entities.Contacts;
using SMT.Domain.Entities.Inventory;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSerial> ProductSerials { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<VendorAdjustment>  VendorAdjustments { get; set; }
        public DbSet<VendorPayment>  VendorPayments { get; set; }
        public DbSet<VendorLedger>  VendorLedgers { get; set; }

        public DbSet<CustomerAdjustment> CustomerAdjustments { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<CustomerLedger> CustomerLedgers { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<PurchaseItemProductSerial> PurchaseItemProductSerials { get; set; }


        public DbSet<SaleInvoice> SaleInvoices { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<SaleItemProductSerial> SaleItemProductSerials { get; set; }

        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalItem> RentalItems { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Brand Configuration ---
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                // One-to-Many: Brand -> Products
                entity.HasMany(e => e.Products)
                      .WithOne(p => p.Brand)
                      .HasForeignKey(p => p.BrandId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Product Configuration ---
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);

                // One-to-Many: Product -> ProductSerials
                entity.HasMany(p => p.ProductSerials) // Assuming you named the collection this
                      .WithOne(ps => ps.Product)
                      .HasForeignKey(ps => ps.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- ProductSerials Configuration ---
            modelBuilder.Entity<ProductSerial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(255);

                //entity.Property(e => e.PurchaseCost).HasColumnType("decimal(8,2)");
                //entity.Property(e => e.SellingCost).HasColumnType("decimal(8,2)");
                //entity.Property(e => e.RentalCost).HasColumnType("decimal(8,2)");

                // Unique constraint: SerialNumber per Tenant
                entity.HasIndex(e => new { e.SerialNumber, e.TenantId }).IsUnique();

                // One-to-Many: ProductSerial -> ProductImages
                entity.HasMany(ps => ps.ProductImages) // Assuming collection name
                      .WithOne(pi => pi.ProductSerial)
                      .HasForeignKey(pi => pi.ProductSerialId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- ProductImage Configuration ---
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            });
        }
        public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

        public override async Task<int> SaveChangesAsync(
   CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.UpdatedAt = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
