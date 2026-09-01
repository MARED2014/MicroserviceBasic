using MediatR;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class UpdateCourierHandler : IRequestHandler<UpdateCourierCommand>
{
    private readonly ICourierRepository _courierRepository;

    public UpdateCourierHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public async Task Handle(UpdateCourierCommand request, CancellationToken cancellationToken)
    {
        var courier = await _courierRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Kurye bulunamadı.");

        if (string.IsNullOrWhiteSpace(request.FullName)
            || string.IsNullOrWhiteSpace(request.Phone)
            || string.IsNullOrWhiteSpace(request.VehiclePlate))
        {
            throw new InvalidOperationException("Kurye adı, telefon ve plaka zorunludur.");
        }

        courier.FullName = request.FullName;
        courier.Phone = request.Phone;
        courier.VehiclePlate = request.VehiclePlate;
        courier.IsActive = request.IsActive;

        _courierRepository.Update(courier);
        await _courierRepository.SaveChangesAsync(cancellationToken);
    }
}
