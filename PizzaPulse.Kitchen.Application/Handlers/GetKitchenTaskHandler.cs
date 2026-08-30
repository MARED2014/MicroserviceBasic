using MediatR;
using PizzaPulse.Kitchen.Application.Queries;
using PizzaPulse.Kitchen.Core.Entities;
using PizzaPulse.Kitchen.Core.Repositories;

namespace PizzaPulse.Kitchen.Application.Handlers;

public class GetKitchenTaskHandler : IRequestHandler<GetKitchenTaskQuery, KitchenTask?>
{
    private readonly IKitchenTaskRepository _kitchenTaskRepository;

    public GetKitchenTaskHandler(IKitchenTaskRepository kitchenTaskRepository)
    {
        _kitchenTaskRepository = kitchenTaskRepository;
    }

    public Task<KitchenTask?> Handle(GetKitchenTaskQuery request, CancellationToken cancellationToken)
    {
        return _kitchenTaskRepository.GetByOrderIdAsync(request.OrderId);
    }
}
