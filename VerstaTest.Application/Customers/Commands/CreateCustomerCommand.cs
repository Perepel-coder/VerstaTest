using MediatR;

namespace VerstaTest.Application.Customers.Commands;

public record CreateCustomerCommand(string login, string password) : IRequest<int?>;
