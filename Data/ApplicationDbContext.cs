using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ITAssetManagement.Models;

namespace ITAssetManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Identity tables are inherited from IdentityDbContext

        // Your IT Asset Management tables
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<AssetVendor> AssetVendors { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for the AssetVendor join table
            modelBuilder.Entity<AssetVendor>()
                .HasKey(av => new { av.AssetID, av.VendorID });

            // (Optional) configure relationships if you want cascade behavior, etc.
            modelBuilder.Entity<AssetVendor>()
                .HasOne(av => av.Asset)
                .WithMany()
                .HasForeignKey(av => av.AssetID);

            modelBuilder.Entity<AssetVendor>()
                .HasOne(av => av.Vendor)
                .WithMany()
                .HasForeignKey(av => av.VendorID);
        }
    }
}
