using Microsoft.AspNetCore.Mvc;
using PizzaPulse.WebUI.Models;
using PizzaPulse.WebUI.Services;

namespace PizzaPulse.WebUI.Controllers;

public class CartController : Controller
{
    private readonly OrderingApiClient _orderingApi;

    public CartController(OrderingApiClient orderingApi)
    {
        _orderingApi = orderingApi;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? customerId, CancellationToken cancellationToken = default)
    {
        var model = new CartPageViewModel { CustomerId = customerId ?? string.Empty };

        if (string.IsNullOrWhiteSpace(customerId))
            return View(model);

        try
        {
            model.Items = await _orderingApi.GetCartAsync(customerId, cancellationToken);
        }
        catch (Exception)
        {
            ViewBag.Error = "Sepet yüklenemedi. Ordering API (http://localhost:5107) çalışıyor olmalı.";
        }

        return View(model);
    }
}
