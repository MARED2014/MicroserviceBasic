using MediatR;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;

namespace PizzaPulse.Ordering.Application.Handlers;

public class GetOrdersByCustomerHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<Order>>
{
    private readonly IOrderingRepository _orderingRepository;

    public GetOrdersByCustomerHandler(IOrderingRepository orderingRepository)
    {
        _orderingRepository = orderingRepository;
    }

    public Task<IEnumerable<Order>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        return _orderingRepository.GetOrdersByCustomerIdAsync(request.CustomerId);
    }
}
