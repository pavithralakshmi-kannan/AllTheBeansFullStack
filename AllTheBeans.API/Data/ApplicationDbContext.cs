using Microsoft.EntityFrameworkCore;
using AllTheBeans.API.Models;

namespace AllTheBeans.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Bean> Beans => Set<Bean>();
        public DbSet<DailySelection> DailySelections => Set<DailySelection>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Bean configuration
            modelBuilder.Entity<Bean>(entity =>
            {
                entity.Property(b => b.Name).HasMaxLength(120).IsRequired();
                entity.Property(b => b.Country).HasMaxLength(80);
                entity.Property(b => b.Colour).HasMaxLength(80);
                entity.Property(b => b.Cost).HasColumnType("decimal(18,2)");

                entity.HasIndex(b => b.Name);
                entity.HasIndex(b => b.Country);
                entity.HasIndex(b => b.Cost);
                entity.HasIndex(b => new { b.Country, b.Name });
            });

            //DailySelection configuration
            modelBuilder.Entity<DailySelection>(entity =>
            {
                entity.HasIndex(d => d.Date).IsUnique();

                entity.HasOne(d => d.Bean) // explicit nav property
                      .WithMany()
                      .HasForeignKey(d => d.BeanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(o => o.OrderDate);

                entity.HasOne(o => o.Bean) 
                      .WithMany()
                      .HasForeignKey(o => o.BeanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}