using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Infrastructure.Contexts;

namespace PizzaPulse.Ordering.Api;

public static class OrderDatabaseSeeder
{
    public static void Seed(OrderDbContext db)
    {
        db.Database.EnsureCreated();

        if (db.PizzaMenus.Any())
            return;

        db.PizzaMenus.AddRange(
            new PizzaMenu
            {
                Id = DemoData.MargheritaId,
                Name = "Margherita",
                Description = "Domates sosu, mozzarella, taze fesleğen",
                BasePrice = 220m,
                IsAvailable = true
            },
            new PizzaMenu
            {
                Id = DemoData.PepperoniId,
                Name = "Pepperoni Supreme",
                Description = "Domates sosu, mozzarella, bol pepperoni",
                BasePrice = 260m,
                IsAvailable = true
            },
            new PizzaMenu
            {
                Id = DemoData.QuattroFormaggiId,
                Name = "Quattro Formaggi",
                Description = "Mozzarella, gorgonzola, parmesan, ricotta",
                BasePrice = 280m,
                IsAvailable = true
            },
            new PizzaMenu
            {
                Id = DemoData.BbqChickenId,
                Name = "BBQ Chicken",
                Description = "BBQ sos, tavuk, soğan, mozzarella",
                BasePrice = 270m,
                IsAvailable = true
            });

        db.SaveChanges();
    }
}
