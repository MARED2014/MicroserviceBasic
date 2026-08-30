using MediatR;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Application.Handlers;

public class CreatePizzaMenuHandler : IRequestHandler<CreatePizzaMenuCommand, Guid>
{
    private readonly IMenuRepository _pizzaMenuRepository;

    public CreatePizzaMenuHandler(IMenuRepository pizzaMenuRepository)
    {
        _pizzaMenuRepository = pizzaMenuRepository;
    }

    public async Task<Guid> Handle(CreatePizzaMenuCommand request, CancellationToken cancellationToken)
    {
        var pizzaMenu = new PizzaMenu
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            BasePrice = request.BasePrice,
            IsAvailable = request.IsAvailable,
            CreatedAt = DateTime.UtcNow
        };

        await _pizzaMenuRepository.AddAsync(pizzaMenu, cancellationToken);
        await _pizzaMenuRepository.SaveChangesAsync(cancellationToken);
        return pizzaMenu.Id;
    }
}
