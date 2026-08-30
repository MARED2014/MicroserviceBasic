using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Infrastructure.Contexts;

namespace PizzaPulse.Delivery.Api;

public static class DemoData
{
    public static readonly Guid CourierAliId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid CourierAyseId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
}

public static class DeliveryDatabaseSeeder
{
    public static void Seed(DeliveryDbContext db)
    {
        db.Database.EnsureCreated();

        if (db.Couriers.Any())
            return;

        db.Couriers.AddRange(
            new Courier
            {
                Id = DemoData.CourierAliId,
                FullName = "Ali Kurye",
                Phone = "05551112233",
                VehiclePlate = "34 ABC 123",
                IsActive = true
            },
            new Courier
            {
                Id = DemoData.CourierAyseId,
                FullName = "Ayşe Kurye",
                Phone = "05554445566",
                VehiclePlate = "34 XYZ 987",
                IsActive = true
            });

        db.SaveChanges();
    }
}
