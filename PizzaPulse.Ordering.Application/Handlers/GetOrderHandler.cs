using MediatR;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;

namespace PizzaPulse.Ordering.Application.Handlers;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, Order?>
{
    private readonly IOrderingRepository _orderingRepository;

    public GetOrderHandler(IOrderingRepository orderingRepository)
    {
        _orderingRepository = orderingRepository;
    }

    public Task<Order?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return _orderingRepository.GetByIdWithItemsAsync(request.Id);
    }
}
