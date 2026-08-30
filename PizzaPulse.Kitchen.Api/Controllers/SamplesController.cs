using Microsoft.AspNetCore.Mvc;

namespace PizzaPulse.Kitchen.Api.Controllers;

[ApiController]
[Route("api/samples")]
[Tags("Samples")]
public class SamplesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        var sampleOrderId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

        return Ok(new
        {
            description = "Ordering POST /api/orders cevabındaki orderId değerini kullanın. Event çalışmazsa bu POST ile iş emri açabilirsiniz.",
            createKitchenTask = new
            {
                orderId = sampleOrderId,
                itemsSummary = new[] { "1x Medium Margherita", "2x Large Pepperoni Supreme" },
                deliveryAddress = "Caferağa Mah. Moda Cad. No:12, Kadıköy/İstanbul"
            },
            startOven = new { path = "/api/kitchen/tasks/{orderId}/start-oven", method = "POST", body = (object?)null },
            markReady = new { path = "/api/kitchen/tasks/{orderId}/ready", method = "POST", body = (object?)null }
        });
    }
}
