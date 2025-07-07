using MediatR;
using Microsoft.AspNetCore.Mvc;
using VerstaTest.Application.Orders.Commands;
using VerstaTest.Application.Orders.Queries;
using VerstaTest.Contract.Orders;
using VerstaTest.Domain;

namespace VerstaTest.WebApi.Controllers;

[Route("/order/")]
public class OrderController : Controller
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    [HttpGet("home/")]
    public IActionResult Home()
    {
        return View();
    }

    [HttpPost("home/create-order/")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
    {
        try
        {
            CreateOrderCommand command = new(
                customerId: orderRequest.customerId,
                senderCity: orderRequest.senderCity,
                senderAddress: orderRequest.senderAddress,
                recipientCity: orderRequest.recipientCity,
                recipientAddress: orderRequest.recipientAddress,
                weight: orderRequest.weight,
                dateCargo: orderRequest.dateCargo);

            int id = await _sender.Send(command);

            CreateOrderResponse response = new(
                id: id,
                customerId: orderRequest.customerId,
                senderCity: orderRequest.senderCity,
                senderAddress: orderRequest.senderAddress,
                recipientCity: orderRequest.recipientCity,
                recipientAddress: orderRequest.recipientAddress,
                weight: orderRequest.weight,
                dateCargo: orderRequest.dateCargo);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("home/get-orders-by-customer/")]
    public async Task<IActionResult> GetOrdersForCustomer([FromQuery] GetOrdersForCustomerRequest ordersForCustomerRequest)
    {
        try
        {
            GetOrdersForCustomerQuery query = new(ordersForCustomerRequest.customerId);

            List<Order> orders = await _sender.Send(query);

            if (!orders.Any())
            {
                return NotFound("No orders found for the specified customer");
            }

            GetOrdersForCustomerResponse response = new GetOrdersForCustomerResponse(
                orders: orders.Select(o => new OrdersForCustomer(
                    id: o.Id,
                    customerId: o.CustomerId,
                    senderCity: o.SenderCity,
                    senderAddress: o.SenderAddress,
                    recipientCity: o.RecipientCity,
                    recipientAddress: o.RecipientAddress,
                    weight: o.Weight,
                    dateCargo: o.DateCargo)).ToList());

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
