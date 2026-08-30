using MediatR;
using PizzaPulse.Kitchen.Application.Commands;
using PizzaPulse.Kitchen.Core.Entities;
using PizzaPulse.Kitchen.Core.Repositories;

namespace PizzaPulse.Kitchen.Application.Handlers;

public class CreateKitchenTaskHandler : IRequestHandler<CreateKitchenTaskCommand, Guid>
{
    private readonly IKitchenTaskRepository _kitchenTaskRepository;

    public CreateKitchenTaskHandler(IKitchenTaskRepository kitchenTaskRepository)
    {
        _kitchenTaskRepository = kitchenTaskRepository;
    }

    public async Task<Guid> Handle(CreateKitchenTaskCommand request, CancellationToken cancellationToken)
    {
        var existing = await _kitchenTaskRepository.GetByOrderIdAsync(request.OrderId);
        if (existing is not null)
            return existing.Id;

        var task = new KitchenTask
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            DeliveryAddress = request.DeliveryAddress,
            ItemsSummary = request.ItemsSummary.ToList(),
            Status = KitchenTaskStatus.Waiting,
            ReceivedAt = DateTime.UtcNow
        };

        await _kitchenTaskRepository.CreateAsync(task);
        return task.Id;
    }
}
