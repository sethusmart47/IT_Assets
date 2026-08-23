using IT_asserts_Claim.entity;
using IT_asserts_Claim.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IT_asserts_Claim.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Accessory> Accessories { get; set; }

        public DbSet<AssetCategory> AssetCategories { get; set; }
        public DbSet<AssetBrand> AssetBrands { get; set; }
        public DbSet<AssetModel> AssetModels { get; set; }

        public DbSet<Vendor>Vendors { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }
        public DbSet<PurchaseAttachment> PurchaseAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AssetCategory>(entity =>
            {
                entity.HasIndex(x => x.CategoryName)
                      .IsUnique();

                entity.HasQueryFilter(x => !x.IsDeleted);
            });


            modelBuilder.Entity<AssetBrand>(entity =>
            {
                entity.HasIndex(x => new
                {
                    x.AssetCategoryId,
                    x.BrandName
                }).IsUnique();

                entity.HasOne(x => x.AssetCategory)
                      .WithMany(x => x.Brands)
                      .HasForeignKey(x => x.AssetCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });


            modelBuilder.Entity<AssetModel>(entity =>
            {
                entity.HasIndex(x => new
                {
                    x.AssetBrandId,
                    x.ModelName
                }).IsUnique();

                // Model -> Category
                entity.HasOne(x => x.AssetCategory)
                      .WithMany(x => x.Models)
                      .HasForeignKey(x => x.AssetCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Model -> Brand
                entity.HasOne(x => x.AssetBrand)
                      .WithMany(x => x.Models)
                      .HasForeignKey(x => x.AssetBrandId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasIndex(e => e.VendorName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.GSTNumber)
                      .IsUnique();
                      

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
            // ─── Purchase Configuration ───
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasIndex(e => e.PurchaseNumber).IsUnique();
                entity.HasIndex(e => e.InvoiceNumber);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.VendorId);

                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(e => e.OwnershipType)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ─── PurchasedItem Configuration ───
            modelBuilder.Entity<PurchasedItem>(entity =>
            {
                entity.HasIndex(e => e.PurchaseId);

                entity.HasOne(e => e.Purchase)
                    .WithMany(p => p.PurchasedItems)
                    .HasForeignKey(e => e.PurchaseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── PurchaseAttachment Configuration ───
            modelBuilder.Entity<PurchaseAttachment>(entity =>
            {
                entity.HasIndex(e => e.PurchaseId);

                entity.HasOne(e => e.Purchase)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(e => e.PurchaseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }

    }

}
