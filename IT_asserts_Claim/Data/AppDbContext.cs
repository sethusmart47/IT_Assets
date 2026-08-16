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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AssetCategory>(entity =>
    {
        entity.HasIndex(e => e.CategoryName).IsUnique();
        entity.HasQueryFilter(e => !e.IsDeleted);
    });

            modelBuilder.Entity<AssetBrand>(entity =>
            {
                entity.HasIndex(e => new
                {
                    e.AssetCategoryId,
                    e.BrandName
                }).IsUnique();

                entity.HasOne(e => e.AssetCategory)
              .WithMany(c => c.Brands)
              .HasForeignKey(e => e.AssetCategoryId)
              .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<AssetModel>(entity =>
            {
                entity.HasIndex(e => new { e.AssetBrandId, e.ModelName }).IsUnique();

                entity.HasOne(e => e.AssetCategory)
                      .WithMany(c => c.Models)
                      .HasForeignKey(e => e.AssetCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AssetBrand)
                      .WithMany(b => b.Models)
                      .HasForeignKey(e => e.AssetBrandId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasIndex(e => e.VendorName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.GSTNumber)
                      .IsUnique();
                      

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

        }
    }
}
