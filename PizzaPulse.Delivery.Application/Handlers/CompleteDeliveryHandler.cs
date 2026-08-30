using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class CompleteDeliveryHandler : IRequestHandler<CompleteDeliveryCommand>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ICourierStateRepository _courierStateRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CompleteDeliveryHandler(
        IDeliveryRepository deliveryRepository,
        ICourierStateRepository courierStateRepository,
        IPublishEndpoint publishEndpoint)
    {
        _deliveryRepository = deliveryRepository;
        _courierStateRepository = courierStateRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(CompleteDeliveryCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _deliveryRepository.GetByOrderIdAsync(request.OrderId)
            ?? throw new InvalidOperationException("Teslimat ataması bulunamadı.");

        if (assignment.Status == DeliveryStatus.Delivered)
            return;

        if (assignment.Status != DeliveryStatus.PickedUp)
            throw new InvalidOperationException("Sipariş önce kurye tarafından alınmalıdır.");

        assignment.Status = DeliveryStatus.Delivered;
        assignment.DeliveredAt = DateTime.UtcNow;
        await _deliveryRepository.SaveChangesAsync();

        var state = await _courierStateRepository.GetCourierStateAsync(assignment.CourierId);
        await _courierStateRepository.SetCourierStateAsync(new ActiveCourierState
        {
            CourierId = assignment.CourierId,
            FullName = state?.FullName ?? string.Empty,
            IsBusy = false,
            LastStatusUpdate = DateTime.UtcNow
        });

        await _publishEndpoint.Publish(new OrderDelivered(assignment.OrderId, assignment.DeliveredAt.Value), cancellationToken);
    }
}
