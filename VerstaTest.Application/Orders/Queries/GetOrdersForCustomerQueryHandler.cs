using MediatR;
using Microsoft.EntityFrameworkCore;
using VerstaTest.Application.Interfaces;
using VerstaTest.Domain;

namespace VerstaTest.Application.Orders.Queries;

public class GetOrdersForCustomerQueryHandler : IRequestHandler<GetOrdersForCustomerQuery, List<Order>>
{
    private readonly IApplicationDbContext _context;

    public GetOrdersForCustomerQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> Handle(GetOrdersForCustomerQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == request.customerId)
            .ToListAsync();
    }
}