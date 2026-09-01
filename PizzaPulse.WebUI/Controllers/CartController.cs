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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CartPageViewModel model, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.CustomerId) || string.IsNullOrWhiteSpace(model.CustomerName) || string.IsNullOrWhiteSpace(model.DeliveryAddress))
        {
            TempData["Error"] = "Müşteri kimliği, ad ve teslimat adresi zorunludur.";
            return RedirectToAction(nameof(Index), new { customerId = model.CustomerId });
        }

        try
        {
            var orderId = await _orderingApi.PlaceOrderAsync(new PlaceOrderRequest
            {
                CustomerId = model.CustomerId,
                CustomerName = model.CustomerName,
                DeliveryAddress = model.DeliveryAddress
            }, cancellationToken);

            TempData["Success"] = "Sipariş alındı. Mutfak kuyruğuna düştü.";
            return RedirectToAction("Details", "Orders", new { id = orderId });
        }
        catch (Exception)
        {
            TempData["Error"] = "Sipariş verilemedi. Sepetin dolu olduğundan ve Ordering API ile RabbitMQ'nun çalıştığından emin olun.";
            return RedirectToAction(nameof(Index), new { customerId = model.CustomerId });
        }
    }
}
