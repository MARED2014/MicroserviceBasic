using MediatR;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class GetCourierHandler : IRequestHandler<GetCourierQuery, Courier?>
{
    private readonly ICourierRepository _courierRepository;

    public GetCourierHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public Task<Courier?> Handle(GetCourierQuery request, CancellationToken cancellationToken)
    {
        return _courierRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
