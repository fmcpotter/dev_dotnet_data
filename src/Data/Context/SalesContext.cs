using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }

        public DbSet<Sale> Sales { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>().HasKey(s => s.Id);
            modelBuilder.Entity<Sale>().OwnsMany(s => s.Items, a =>
            {
                a.WithOwner().HasForeignKey("SaleId");
                a.Property<Guid>("Id");
                a.HasKey("Id");
                a.Property(i => i.ProductExternalId);
                a.Property(i => i.ProductDescription);
                a.Property(i => i.Quantity);
                a.Property(i => i.UnitPrice);
                a.Property(i => i.Discount);
                a.Property(i => i.Total);
                a.Property(i => i.IsCancelled);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}