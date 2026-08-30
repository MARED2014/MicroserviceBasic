using Microsoft.AspNetCore.Mvc;

namespace PizzaPulse.Delivery.Api.Controllers;

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
            description = "Sipariş piştikten sonra (OrderBaked) kurye otomatik atanır. Event yoksa POST /api/deliveries/assign kullanın. Pickup ve complete path parametresi orderId ister, body boştur.",
            assignCourier = new
            {
                orderId = sampleOrderId,
                customerAddress = "Caferağa Mah. Moda Cad. No:12, Kadıköy/İstanbul"
            },
            pickup = new { path = "/api/deliveries/{orderId}/pickup", method = "POST", body = (object?)null },
            complete = new { path = "/api/deliveries/{orderId}/complete", method = "POST", body = (object?)null }
        });
    }
}
