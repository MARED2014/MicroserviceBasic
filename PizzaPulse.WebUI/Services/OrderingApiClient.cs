using PizzaPulse.WebUI.Models;

namespace PizzaPulse.WebUI.Services;

public class OrderingApiClient
{
    private readonly HttpClient _httpClient;

    public OrderingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PizzaMenuDto>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var menu = await _httpClient.GetFromJsonAsync<List<PizzaMenuDto>>("/api/menu", cancellationToken);
        return menu ?? [];
    }

    public async Task<List<CartItemDto>> GetCartAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var cart = await _httpClient.GetFromJsonAsync<List<CartItemDto>>(
            $"/api/cart?customerId={Uri.EscapeDataString(customerId)}",
            cancellationToken);

        return cart ?? [];
    }

    public async Task AddToCartAsync(AddToCartRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("/api/cart/items", request, cancellationToken);
        if (response.IsSuccessStatusCode)
            return;

        var detail = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail) ? "Sepete eklenemedi." : detail);
    }

    public async Task<Guid> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("/api/orders", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var detail = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail) ? "Sipariş oluşturulamadı." : detail);
        }

        var payload = await response.Content.ReadFromJsonAsync<PlaceOrderResponse>(cancellationToken);
        if (payload is null || payload.OrderId == Guid.Empty)
            throw new InvalidOperationException("Sipariş numarası alınamadı.");

        return payload.OrderId;
    }

    public async Task<OrderDto?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<OrderDto>($"/api/orders/{orderId}", cancellationToken);
    }
}
