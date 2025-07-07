namespace VerstaTest.Contract.Orders;

public record CreateOrderResponse(
    int id,
    int customerId,
    string senderCity,
    string senderAddress,
    string recipientCity,
    string recipientAddress,
    double weight,
    DateTime dateCargo);
