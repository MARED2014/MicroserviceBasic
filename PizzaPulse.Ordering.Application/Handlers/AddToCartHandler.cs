using MediatR;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;

namespace PizzaPulse.Ordering.Application.Handlers;

public class AddToCartHandler : IRequestHandler<AddToCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMenuRepository _menuRepository;

    public AddToCartHandler(ICartRepository cartRepository, IMenuRepository menuRepository)
    {
        _cartRepository = cartRepository;
        _menuRepository = menuRepository;
    }

    public async Task Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerId))
            throw new InvalidOperationException("Müşteri bilgisi zorunludur.");

        if (request.Quantity <= 0)
            throw new InvalidOperationException("Adet en az 1 olmalıdır.");

        var menu = await _menuRepository.GetByIdAsync(request.PizzaMenuId, cancellationToken);
        if (menu is null || !menu.IsAvailable)
            throw new InvalidOperationException("Pizza menüde bulunamadı veya satışta değil.");

        var size = string.IsNullOrWhiteSpace(request.Size) ? "Medium" : request.Size;

        await _cartRepository.AddOrUpdateCartItemAsync(request.CustomerId, new CartItem
        {
            PizzaMenuId = menu.Id,
            PizzaName = menu.Name,
            Quantity = request.Quantity,
            Size = size,
            UnitPrice = menu.BasePrice
        });
    }
}
