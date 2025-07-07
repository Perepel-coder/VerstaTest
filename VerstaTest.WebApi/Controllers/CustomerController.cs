using MediatR;
using Microsoft.AspNetCore.Mvc;
using VerstaTest.Application.Customers.Commands;
using VerstaTest.Contract.Customers;

namespace VerstaTest.WebApi.Controllers;

[Route("/customer/")]
public class CustomerController : Controller
{
    private readonly ISender _sender;

    public CustomerController(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    [HttpGet("authorization/")]
    public IActionResult Authorization()
    {
        return View();
    }

    [HttpPost("authorization/create-customer/")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest customerRequest)
    {
        CreateCustomerCommand command = new(customerRequest.login, customerRequest.password);

        try
        {
            int? id = await _sender.Send(command);

            CreateCustomerResponse response = new(id ?? -1);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("authorization/check-customer/")]
    public async Task<IActionResult> CheckCustomer([FromBody] CheckCustomerRequest customerRequest)
    {
        CheckCustomerCommand command = new(customerRequest.login, customerRequest.password);

        try
        {
            int? id = await _sender.Send(command);

            CreateCustomerResponse response = new(id ?? -1);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
