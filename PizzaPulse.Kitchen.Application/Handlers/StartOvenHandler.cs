using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Kitchen.Application.Commands;
using PizzaPulse.Kitchen.Core.Entities;
using PizzaPulse.Kitchen.Core.Repositories;

namespace PizzaPulse.Kitchen.Application.Handlers;

public class StartOvenHandler : IRequestHandler<StartOvenCommand>
{
    private readonly IKitchenTaskRepository _kitchenTaskRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public StartOvenHandler(IKitchenTaskRepository kitchenTaskRepository, IPublishEndpoint publishEndpoint)
    {
        _kitchenTaskRepository = kitchenTaskRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StartOvenCommand request, CancellationToken cancellationToken)
    {
        var task = await _kitchenTaskRepository.GetByOrderIdAsync(request.OrderId)
            ?? throw new InvalidOperationException("Mutfak işi bulunamadı.");

        if (task.Status == KitchenTaskStatus.Ready)
            throw new InvalidOperationException("Hazır sipariş tekrar fırına alınamaz.");

        if (task.Status == KitchenTaskStatus.InOven)
            return;

        task.Status = KitchenTaskStatus.InOven;
        task.OvenStartedAt = DateTime.UtcNow;
        await _kitchenTaskRepository.UpdateByOrderIdAsync(task);

        await _publishEndpoint.Publish(new OrderPreparationStarted(task.OrderId, task.OvenStartedAt.Value), cancellationToken);
    }
}
