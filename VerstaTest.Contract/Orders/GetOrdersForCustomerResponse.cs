namespace VerstaTest.Contract.Orders;

public record GetOrdersForCustomerResponse(IEnumerable<OrdersForCustomer> orders);

public record OrdersForCustomer(
    int id,
    int customerId,
    string senderCity,
    string senderAddress,
    string recipientCity,
    string recipientAddress,
    double weight,
    DateTime dateCargo);