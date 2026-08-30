using MediatR;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Application.Handlers;

public class GetMenuHandler : IRequestHandler<GetMenuQuery, PizzaMenu?>
{
    public readonly IMenuRepository _repository;

    public GetMenuHandler(IMenuRepository repository)
    {
        _repository = repository;
    }

    public Task<PizzaMenu?> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
