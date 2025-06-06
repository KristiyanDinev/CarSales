using CarSales.Models.Database;
using CarSales.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Database
{
    public class DatabaseContext: IdentityDbContext<IdentityUserModel>
    {
        public DbSet<CarModel> Cars { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : 
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarModel>().ToTable("Cars");

            modelBuilder.Entity<CarModel>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CarModel>()
                .Property(c => c.Make)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<CarModel>()
                .Property(c => c.Model)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<CarModel>()
                .Property(c => c.Year)
                .IsRequired();

            modelBuilder.Entity<CarModel>()
                .Property(c => c.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CarModel>()
                .Property(c => c.Color)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<CarModel>()
                .Property(c => c.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<CarModel>()
                .Property(c => c.ImageUrl)
                .HasMaxLength(200);
        }

    }
}
