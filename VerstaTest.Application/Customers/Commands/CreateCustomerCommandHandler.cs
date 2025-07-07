using MediatR;
using VerstaTest.Application.Interfaces;
using VerstaTest.Domain;

namespace VerstaTest.Application.Customers.Commands;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int?>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int?> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer? anyCustomer = _context.Customers.SingleOrDefault(c => c.Login == request.login);

        if(anyCustomer != null) return null;

        Customer customer = new()
        {
            Login = request.login,
            Password = request.password
        };

        await _context.Customers.AddAsync(customer);

        await _context.SaveChangesAsync();

        return customer.Id;
    }
}
