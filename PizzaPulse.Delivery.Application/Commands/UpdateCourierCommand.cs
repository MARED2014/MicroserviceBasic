using MediatR;

namespace PizzaPulse.Delivery.Application.Commands;

public record UpdateCourierCommand(Guid Id, string FullName, string Phone, string VehiclePlate, bool IsActive) : IRequest;
