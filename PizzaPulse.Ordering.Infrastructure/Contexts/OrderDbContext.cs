using Microsoft.EntityFrameworkCore;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Infrastructure.Contexts;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Core.Entities.Order> Orders => Set<Core.Entities.Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PizzaMenu> PizzaMenus => Set<PizzaMenu>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Entity Konfigürasyonları ve İlişkileri

        // Order Configuration
        modelBuilder.Entity<Core.Entities.Order>(builder =>
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .HasConversion<int>(); // Enum'ı integer olarak saklar

            // Order -> OrderItem (One-to-Many)
            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItem Configuration
        modelBuilder.Entity<OrderItem>(builder =>
        {
            builder.ToTable("OrderItems");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.PizzaName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Size)
                .HasMaxLength(20);
        });

        // PizzaMenu Configuration
        modelBuilder.Entity<PizzaMenu>(builder =>
        {
            builder.ToTable("PizzaMenus");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.BasePrice)
                .HasColumnType("decimal(18,2)");
        });
    }
}
