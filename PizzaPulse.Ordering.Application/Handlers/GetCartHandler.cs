using MediatR;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;

namespace PizzaPulse.Ordering.Application.Handlers;

public class GetCartHandler : IRequestHandler<GetCartQuery, IReadOnlyList<CartItem>>
{
    private readonly ICartRepository _cartRepository;

    public GetCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<IReadOnlyList<CartItem>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        return await _cartRepository.GetCartAsync(request.CustomerId);
    }
}
