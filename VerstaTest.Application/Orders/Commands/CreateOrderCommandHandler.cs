using MediatR;
using VerstaTest.Application.Interfaces;
using VerstaTest.Domain;

namespace VerstaTest.Application.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            CustomerId = request.customerId,
            SenderCity = request.senderCity,
            SenderAddress = request.senderAddress,
            RecipientCity = request.recipientCity,
            RecipientAddress = request.recipientAddress,
            Weight = request.weight,
            DateCargo = request.dateCargo
        };

        await _context.Orders.AddAsync(order);

        await _context.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}