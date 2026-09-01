using MediatR;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class DeleteCourierHandler : IRequestHandler<DeleteCourierCommand>
{
    private readonly ICourierRepository _courierRepository;

    public DeleteCourierHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public async Task Handle(DeleteCourierCommand request, CancellationToken cancellationToken)
    {
        var courier = await _courierRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Kurye bulunamadı.");

        courier.IsActive = false;
        _courierRepository.Update(courier);
        await _courierRepository.SaveChangesAsync(cancellationToken);
    }
}
