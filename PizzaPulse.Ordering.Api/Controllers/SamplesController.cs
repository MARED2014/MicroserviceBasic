using Microsoft.AspNetCore.Mvc;

namespace PizzaPulse.Ordering.Api.Controllers;

[ApiController]
[Route("api/samples")]
[Tags("Samples")]
public class SamplesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new
        {
            description = "Swagger Try it out için kopyalanacak gövdeler. Önce GET /api/menu, sonra POST /api/cart/items (iki kez), ardından GET /api/cart ve POST /api/orders.",
            createPizzaMenu = new
            {
                name = "Diavola",
                description = "Acılı salam, mozzarella, pul biber",
                basePrice = 250,
                isAvailable = true
            },
            addToCartMargherita = new
            {
                customerId = DemoData.CustomerId,
                pizzaMenuId = DemoData.MargheritaId,
                quantity = 1,
                size = "Medium"
            },
            addToCartPepperoni = new
            {
                customerId = DemoData.CustomerId,
                pizzaMenuId = DemoData.PepperoniId,
                quantity = 2,
                size = "Large"
            },
            placeOrder = new
            {
                customerId = DemoData.CustomerId,
                customerName = DemoData.CustomerName,
                deliveryAddress = DemoData.DeliveryAddress
            }
        });
    }
}
