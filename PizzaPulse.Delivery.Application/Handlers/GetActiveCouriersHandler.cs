using MediatR;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class GetActiveCouriersHandler : IRequestHandler<GetActiveCouriersQuery, IEnumerable<Courier>>
{
    private readonly IDeliveryRepository _deliveryRepository;

    public GetActiveCouriersHandler(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public Task<IEnumerable<Courier>> Handle(GetActiveCouriersQuery request, CancellationToken cancellationToken)
    {
        return _deliveryRepository.GetActiveCouriersAsync();
    }
}
