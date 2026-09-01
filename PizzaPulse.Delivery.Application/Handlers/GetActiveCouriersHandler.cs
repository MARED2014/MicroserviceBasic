using MediatR;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class GetActiveCouriersHandler : IRequestHandler<GetActiveCouriersQuery, IEnumerable<Courier>>
{
    private readonly ICourierRepository _courierRepository;

    public GetActiveCouriersHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public Task<IEnumerable<Courier>> Handle(GetActiveCouriersQuery request, CancellationToken cancellationToken)
    {
        return _courierRepository.GetActiveAsync(cancellationToken);
    }
}
