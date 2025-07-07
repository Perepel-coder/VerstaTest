using MediatR;

namespace VerstaTest.Application.Customers.Commands;

public record CheckCustomerCommand(string login, string password) : IRequest<int?>;
