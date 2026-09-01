using MediatR;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;

namespace PizzaPulse.Delivery.Application.Handlers;

public class AssignCourierHandler : IRequestHandler<AssignCourierCommand, Guid>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ICourierRepository _courierRepository;
    private readonly ICourierStateRepository _courierStateRepository;

    public AssignCourierHandler(
        IDeliveryRepository deliveryRepository,
        ICourierRepository courierRepository,
        ICourierStateRepository courierStateRepository)
    {
        _deliveryRepository = deliveryRepository;
        _courierRepository = courierRepository;
        _courierStateRepository = courierStateRepository;
    }

    public async Task<Guid> Handle(AssignCourierCommand request, CancellationToken cancellationToken)
    {
        var existing = await _deliveryRepository.GetByOrderIdAsync(request.OrderId);
        if (existing is not null)
            return existing.Id;

        if (string.IsNullOrWhiteSpace(request.CustomerAddress))
            throw new InvalidOperationException("Teslimat adresi zorunludur.");

        var courier = await FindAvailableCourierAsync()
            ?? throw new InvalidOperationException("Müsait kurye bulunamadı.");

        var assignment = new DeliveryAssignment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            CourierId = courier.Id,
            CustomerAddress = request.CustomerAddress,
            Status = DeliveryStatus.Assigned,
            AssignedAt = DateTime.UtcNow
        };

        await _deliveryRepository.AddAssignmentAsync(assignment);
        await _deliveryRepository.SaveChangesAsync();

        await _courierStateRepository.SetCourierStateAsync(new ActiveCourierState
        {
            CourierId = courier.Id,
            FullName = courier.FullName,
            IsBusy = true,
            LastStatusUpdate = DateTime.UtcNow
        });

        return assignment.Id;
    }

    private async Task<Courier?> FindAvailableCourierAsync()
    {
        var couriers = await _courierRepository.GetActiveAsync();

        foreach (var courier in couriers)
        {
            var state = await _courierStateRepository.GetCourierStateAsync(courier.Id);
            if (state is null || !state.IsBusy)
                return courier;
        }

        return null;
    }
}
