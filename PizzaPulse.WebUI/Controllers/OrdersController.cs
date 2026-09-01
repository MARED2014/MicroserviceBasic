using Microsoft.AspNetCore.Mvc;
using PizzaPulse.WebUI.Services;

namespace PizzaPulse.WebUI.Controllers;

public class OrdersController : Controller
{
    private readonly OrderingApiClient _orderingApi;

    public OrdersController(OrderingApiClient orderingApi)
    {
        _orderingApi = orderingApi;
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderingApi.GetOrderAsync(id, cancellationToken);
            if (order is null)
                return NotFound();

            return View(order);
        }
        catch (Exception)
        {
            ViewBag.Error = "Sipariş yüklenemedi. Ordering API çalışıyor olmalı.";
            return View();
        }
    }
}
