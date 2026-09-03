using ITAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }

        public DbSet<AssetCategory> AssetCategories { get; set; }
        public DbSet<AssetBrand> AssetBrands { get; set; }
        public DbSet<AssetModel> AssetModels { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }
        public DbSet<PurchaseAttachment> PurchaseAttachments { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetLifecycleHistory> AssetLifecycleHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AssetCategory>(entity =>
            {
                entity.HasIndex(x => x.CategoryName)
                      .IsUnique()
                       .HasFilter("[IsDeleted] = 0");
                entity.HasQueryFilter(x => !x.IsDeleted);
            });


            modelBuilder.Entity<AssetBrand>(entity =>
            {
                entity.HasIndex(x => new
                {
                    x.AssetCategoryId,
                    x.BrandName
                }).IsUnique().HasFilter("[IsDeleted] = 0");

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
                }).IsUnique().HasFilter("[IsDeleted] = 0");

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
            modelBuilder.Entity<Asset>(entity =>
             {
                 entity.HasIndex(a => a.AssetTag).IsUnique();
                 entity.HasIndex(a => a.SerialNumber).IsUnique();
                 entity.HasQueryFilter(a => !a.IsDeleted);

                 entity.HasOne(a => a.Purchase)
                       .WithMany()
                       .HasForeignKey(a => a.PurchaseId)
                       .OnDelete(DeleteBehavior.Restrict);
             });

            modelBuilder.Entity<AssetLifecycleHistory>(entity =>
            {
                entity.HasQueryFilter(h => !h.IsDeleted);

                entity.HasOne(h => h.Asset)
                      .WithMany(a => a.LifecycleHistories)
                      .HasForeignKey(h => h.AssetId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(h => h.AssetId);
                entity.HasIndex(h => new { h.AssetId, h.PerformedDate });
            });
            //Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasQueryFilter(e => !e.IsDeleted);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // AssetAssignment
            modelBuilder.Entity<AssetAssignment>(entity =>
            {
                entity.HasQueryFilter(a => !a.IsDeleted);
            });

            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasQueryFilter(e => !e.IsDeleted);
                entity.HasIndex(e => new { e.AssetId, e.Status });
            });

        }


       
    }

}