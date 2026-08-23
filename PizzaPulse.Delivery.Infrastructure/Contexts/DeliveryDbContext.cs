using Microsoft.EntityFrameworkCore;
using PizzaPulse.Delivery.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Delivery.Infrastructure.Contexts;

public class DeliveryDbContext : DbContext
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
    {
    }

    public DbSet<Courier> Couriers => Set<Courier>();
    public DbSet<DeliveryAssignment> DeliveryAssignments => Set<DeliveryAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Courier Configuration
        modelBuilder.Entity<Courier>(builder =>
        {
            builder.ToTable("Couriers");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.VehiclePlate)
                .IsRequired()
                .HasMaxLength(20);
        });

        // DeliveryAssignment Configuration
        modelBuilder.Entity<DeliveryAssignment>(builder =>
        {
            builder.ToTable("DeliveryAssignments");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.CustomerAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.Status)
                .HasConversion<int>(); // Enum değerini integer olarak saklar

            // Relationships
            builder.HasOne<Courier>()
                .WithMany()
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
