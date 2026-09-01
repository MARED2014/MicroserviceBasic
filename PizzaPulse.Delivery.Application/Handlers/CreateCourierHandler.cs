using MediatR;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class CreateCourierHandler : IRequestHandler<CreateCourierCommand, Guid>
{
    private readonly ICourierRepository _courierRepository;

    public CreateCourierHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public async Task<Guid> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FullName)
            || string.IsNullOrWhiteSpace(request.Phone)
            || string.IsNullOrWhiteSpace(request.VehiclePlate))
        {
            throw new InvalidOperationException("Kurye adı, telefon ve plaka zorunludur.");
        }

        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Phone = request.Phone,
            VehiclePlate = request.VehiclePlate,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _courierRepository.AddAsync(courier, cancellationToken);
        await _courierRepository.SaveChangesAsync(cancellationToken);
        return courier.Id;
    }
}
