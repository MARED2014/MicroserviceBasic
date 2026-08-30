using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class PickupDeliveryHandler : IRequestHandler<PickupDeliveryCommand>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ICourierStateRepository _courierStateRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public PickupDeliveryHandler(
        IDeliveryRepository deliveryRepository,
        ICourierStateRepository courierStateRepository,
        IPublishEndpoint publishEndpoint)
    {
        _deliveryRepository = deliveryRepository;
        _courierStateRepository = courierStateRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(PickupDeliveryCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _deliveryRepository.GetByOrderIdAsync(request.OrderId)
            ?? throw new InvalidOperationException("Teslimat ataması bulunamadı.");

        if (assignment.Status == DeliveryStatus.PickedUp || assignment.Status == DeliveryStatus.Delivered)
            return;

        if (assignment.Status != DeliveryStatus.Assigned)
            throw new InvalidOperationException("Sipariş teslim alınacak durumda değil.");

        assignment.Status = DeliveryStatus.PickedUp;
        await _deliveryRepository.SaveChangesAsync();

        var state = await _courierStateRepository.GetCourierStateAsync(assignment.CourierId);
        var courierName = state?.FullName ?? string.Empty;

        await _publishEndpoint.Publish(
            new OrderOnTheWay(assignment.OrderId, assignment.CourierId, courierName, DateTime.UtcNow),
            cancellationToken);
    }
}
