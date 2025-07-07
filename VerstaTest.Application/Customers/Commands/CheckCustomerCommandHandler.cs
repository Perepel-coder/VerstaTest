using MediatR;
using VerstaTest.Application.Interfaces;
using VerstaTest.Domain;

namespace VerstaTest.Application.Customers.Commands;

public class CheckCustomerCommandHandler : IRequestHandler<CheckCustomerCommand, int?>
{
    private readonly IApplicationDbContext _context;

    public CheckCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int?> Handle(CheckCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = new()
        {
            Login = request.login,
            Password = request.password
        };

        int? customerId = _context.Customers.SingleOrDefault(c =>
        c.Login == customer.Login &&
        c.Password == customer.Password)?.Id;

        return customerId;
    }
}
