using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Kitchen.Application.Commands;
using PizzaPulse.Kitchen.Core.Entities;
using PizzaPulse.Kitchen.Core.Repositories;

namespace PizzaPulse.Kitchen.Application.Handlers;

public class MarkOrderReadyHandler : IRequestHandler<MarkOrderReadyCommand>
{
    private readonly IKitchenTaskRepository _kitchenTaskRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public MarkOrderReadyHandler(IKitchenTaskRepository kitchenTaskRepository, IPublishEndpoint publishEndpoint)
    {
        _kitchenTaskRepository = kitchenTaskRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(MarkOrderReadyCommand request, CancellationToken cancellationToken)
    {
        var task = await _kitchenTaskRepository.GetByOrderIdAsync(request.OrderId)
            ?? throw new InvalidOperationException("Mutfak işi bulunamadı.");

        if (task.Status == KitchenTaskStatus.Ready)
            return;

        if (task.Status != KitchenTaskStatus.InOven)
            throw new InvalidOperationException("Sipariş önce fırına alınmalıdır.");

        task.Status = KitchenTaskStatus.Ready;
        task.BakedAt = DateTime.UtcNow;
        await _kitchenTaskRepository.UpdateByOrderIdAsync(task);

        await _publishEndpoint.Publish(new OrderBaked(task.OrderId, task.DeliveryAddress, task.BakedAt.Value), cancellationToken);
    }
}
