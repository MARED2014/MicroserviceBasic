using MediatR;
using PizzaPulse.Kitchen.Application.Queries;
using PizzaPulse.Kitchen.Core.Entities;
using PizzaPulse.Kitchen.Core.Repositories;

namespace PizzaPulse.Kitchen.Application.Handlers;

public class GetPendingKitchenTasksHandler : IRequestHandler<GetPendingKitchenTasksQuery, IEnumerable<KitchenTask>>
{
    private readonly IKitchenTaskRepository _kitchenTaskRepository;

    public GetPendingKitchenTasksHandler(IKitchenTaskRepository kitchenTaskRepository)
    {
        _kitchenTaskRepository = kitchenTaskRepository;
    }

    public async Task<IEnumerable<KitchenTask>> Handle(GetPendingKitchenTasksQuery request, CancellationToken cancellationToken)
    {
        var pendingTasks = await _kitchenTaskRepository.GetPendingTasksAsync();
        return pendingTasks;
    }
}
