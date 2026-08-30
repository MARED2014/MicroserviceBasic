using Microsoft.AspNetCore.Mvc;
using PizzaPulse.WebUI.Models;
using PizzaPulse.WebUI.Services;

namespace PizzaPulse.WebUI.Controllers;

public class MenuController : Controller
{
    private readonly OrderingApiClient _orderingApi;

    public MenuController(OrderingApiClient orderingApi)
    {
        _orderingApi = orderingApi;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? customerId, CancellationToken cancellationToken)
    {
        var model = new MenuPageViewModel { CustomerId = customerId ?? string.Empty };

        try
        {
            var menu = await _orderingApi.GetMenuAsync(cancellationToken);
            model.Items = menu.Select(item => new MenuSelectionItem
            {
                PizzaMenuId = item.Id,
                Name = item.Name,
                Description = item.Description,
                BasePrice = item.BasePrice,
                Quantity = 1,
                Size = "Medium"
            }).ToList();
        }
        catch (Exception)
        {
            ViewBag.Error = "Menü yüklenemedi. Ordering API (http://localhost:5107) çalışıyor olmalı.";
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSelected(MenuPageViewModel model, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.CustomerId))
        {
            TempData["Error"] = "Müşteri kimliği girin.";
            return RedirectToAction(nameof(Index));
        }

        var selected = model.Items.Where(item => item.Selected).ToList();
        if (selected.Count == 0)
        {
            TempData["Error"] = "Sepete eklemek için en az bir pizza seçin.";
            return RedirectToAction(nameof(Index), new { customerId = model.CustomerId });
        }

        try
        {
            foreach (var item in selected)
            {
                await _orderingApi.AddToCartAsync(new AddToCartRequest
                {
                    CustomerId = model.CustomerId,
                    PizzaMenuId = item.PizzaMenuId,
                    Quantity = item.Quantity < 1 ? 1 : item.Quantity,
                    Size = string.IsNullOrWhiteSpace(item.Size) ? "Medium" : item.Size
                }, cancellationToken);
            }

            TempData["Success"] = $"{selected.Count} ürün sepete eklendi.";
            return RedirectToAction("Index", "Cart", new { customerId = model.CustomerId });
        }
        catch (Exception)
        {
            TempData["Error"] = "Sepete eklenemedi. Ordering API ve Redis çalışıyor olmalı.";
            return RedirectToAction(nameof(Index));
        }
    }
}
