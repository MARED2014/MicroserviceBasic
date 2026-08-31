using MediatR;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Repositories;

namespace PizzaPulse.Ordering.Application.Handlers;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly IOrderingRepository _orderingRepository;

    public UpdateOrderStatusHandler(IOrderingRepository orderingRepository)
    {
        _orderingRepository = orderingRepository;
    }

    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderingRepository.GetByIdAsync(request.OrderId) ?? throw new InvalidOperationException("Sipariş bulunamadı.");

        order.Status = request.Status;
        _orderingRepository.Update(order);
        await _orderingRepository.SaveChangesAsync();
    }
}
