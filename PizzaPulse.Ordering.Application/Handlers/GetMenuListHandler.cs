using MediatR;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Application.Handlers;

public class GetMenuListHandler : IRequestHandler<GetMenuListQuery, IEnumerable<PizzaMenu>>
{
    private readonly IMenuRepository _pizzaMenuRepository;

    public GetMenuListHandler(IMenuRepository pizzaMenuRepository)
    {
        _pizzaMenuRepository = pizzaMenuRepository;
    }

    public async Task<IEnumerable<PizzaMenu>> Handle(GetMenuListQuery request, CancellationToken cancellationToken)
    {
        var menuList = await _pizzaMenuRepository.GetAllAvailableAsync(cancellationToken);
        return menuList;
    }
}
