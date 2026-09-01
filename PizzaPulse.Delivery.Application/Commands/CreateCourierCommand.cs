using MediatR;

namespace PizzaPulse.Delivery.Application.Commands;

public record CreateCourierCommand(string FullName, string Phone, string VehiclePlate, bool IsActive = true) : IRequest<Guid>;
