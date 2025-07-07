using MediatR;
using VerstaTest.Domain;

namespace VerstaTest.Application.Orders.Queries;

public record GetOrdersForCustomerQuery(int customerId) : IRequest<List<Order>>;