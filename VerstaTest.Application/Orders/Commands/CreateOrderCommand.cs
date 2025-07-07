using MediatR;

namespace VerstaTest.Application.Orders.Commands;

public record CreateOrderCommand(
    int customerId,
    string senderCity,
    string senderAddress,
    string recipientCity,
    string recipientAddress,
    double weight,
    DateTime dateCargo) : IRequest<int>;