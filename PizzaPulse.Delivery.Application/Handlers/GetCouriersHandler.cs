using MediatR;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class GetCouriersHandler : IRequestHandler<GetCouriersQuery, IEnumerable<Courier>>
{
    private readonly ICourierRepository _courierRepository;

    public GetCouriersHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public Task<IEnumerable<Courier>> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
    {
        return _courierRepository.GetAllAsync(cancellationToken);
    }
}
