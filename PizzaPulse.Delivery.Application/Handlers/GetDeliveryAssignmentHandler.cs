using MediatR;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class GetDeliveryAssignmentHandler : IRequestHandler<GetDeliveryAssignmentQuery, DeliveryAssignment?>
{
    private readonly IDeliveryRepository _deliveryRepository;

    public GetDeliveryAssignmentHandler(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public Task<DeliveryAssignment?> Handle(GetDeliveryAssignmentQuery request, CancellationToken cancellationToken)
    {
        return _deliveryRepository.GetByOrderIdAsync(request.OrderId);
    }
}
