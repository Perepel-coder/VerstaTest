namespace VerstaTest.Contract.Orders;

public record CreateOrderRequest(
    int customerId,
    string senderCity,
    string senderAddress,
    string recipientCity,
    string recipientAddress,
    double weight,
    DateTime dateCargo);